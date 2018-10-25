CREATE TABLE posts (
    "id"                INTEGER PRIMARY KEY,
    creation_date       TIMESTAMP WITHOUT TIME ZONE,
    score               INTEGER,
    body                TEXT,
    author_id           INTEGER
);

CREATE TABLE posts_question (
    "id"                INTEGER PRIMARY KEY,
    accepted_answer_id  INTEGER,
    closed_date         TIMESTAMP WITHOUT TIME ZONE,
    title               TEXT
);

CREATE TABLE posts_answer (
    "id"                INTEGER PRIMARY KEY,
    parent_id           INTEGER
);

INSERT INTO posts
SELECT DISTINCT ON (p.id)
    p.id,
    p.creationdate,
    p.score,
    p.body,
    p.ownerid
FROM posts_universal p;

INSERT INTO posts_question
SELECT DISTINCT ON (p.id)
    p.id,
    p.acceptedanswerid,
    p.closeddate,
    p.title
FROM posts_universal p
WHERE p.posttypeid = 1;

INSERT INTO posts_answer
SELECT DISTINCT ON (p.id)
    p.id,
    p.parentid
FROM posts_universal p
WHERE p.posttypeid = 2;

-- set "accepted_answer_id" that does not exist to null
UPDATE posts_question
SET accepted_answer_id = null
WHERE accepted_answer_id NOT IN (
    SELECT "id" FROM ONLY posts
);

ALTER TABLE posts
    ADD FOREIGN KEY (author_id)
    REFERENCES authors ("id");

ALTER TABLE posts_question
    ADD FOREIGN KEY (accepted_answer_id)
    REFERENCES posts ("id");

ALTER TABLE posts_answer
    ADD FOREIGN KEY (parent_id)
    REFERENCES posts ("id");

ALTER TABLE posts_question
    ADD COLUMN title_tokens TSVECTOR;

UPDATE posts_question p1
    SET title_tokens = to_tsvector(p1.title)
    FROM posts_question p2;

--
-- SELECT TESTS
--
SELECT * FROM posts join posts_question using("id");
SELECT * FROM posts join posts_answer using("id");


SELECT DISTINCT
    pu."id" post_id,
	pu.linkpostid link_id
INTO posts_link
FROM posts_universal pu
WHERE pu.linkpostid IS NOT NULL
AND pu.linkpostid NOT IN (
    SELECT linkpostid
    FROM posts_universal
    WHERE linkpostid IS NOT NULL
    except (
        SELECT id FROM posts_universal
    )
);

ALTER TABLE posts_link
    ADD FOREIGN KEY (post_id)
    REFERENCES posts(id);

ALTER TABLE posts_link
    ADD FOREIGN KEY (link_id)
    REFERENCES posts(id);
