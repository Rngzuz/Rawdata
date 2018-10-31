--
-- AUTHORS
--
SELECT
    t1.ownerid              "id",
    t1.ownerdisplayname     display_name,
    t1.ownercreationdate    creation_date,
    t1.ownerlocation        "location",
    t1.ownerage             age
INTO authors
FROM (
    SELECT DISTINCT ON (ownerid)
        ownerid,
        ownerdisplayname,
        ownercreationdate,
        ownerlocation,
        ownerage
    FROM posts_universal
) t1;

ALTER TABLE authors
ADD PRIMARY KEY ("id");

INSERT INTO authors (
    "id",
    display_name,
    creation_date,
    "location",
    age
)
SELECT
    t2.authorid             "id",
    t2.authordisplayname    display_name,
    t2.authorcreationdate   creation_date,
    t2.authorlocation       "location",
    t2.authorage            age
FROM (
    SELECT DISTINCT ON (authorid)
        authorid,
        authordisplayname,
        authorcreationdate,
        authorlocation,
        authorage
    FROM comments_universal
) t2
WHERE t2.authorid NOT IN (
    SELECT id FROM authors
);

--
-- POSTS
--
CREATE TABLE posts (
    "id"                INTEGER PRIMARY KEY,
    "type_id"           SMALLINT,
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
    p.posttypeid,
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
WHERE accepted_answer_id NOT IN (SELECT "id" FROM posts);

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
-- POSTS_LINK
--
SELECT DISTINCT
    p."id"              post_id,
	p.linkpostid        link_id
INTO posts_link
FROM posts_universal p
WHERE p.linkpostid IS NOT NULL
AND p.linkpostid NOT IN (
    SELECT linkpostid
    FROM posts_universal
    WHERE linkpostid IS NOT NULL
    except (SELECT id FROM posts_universal)
);

ALTER TABLE posts_link
    ADD FOREIGN KEY (post_id)
    REFERENCES posts(id);

ALTER TABLE posts_link
    ADD FOREIGN KEY (link_id)
    REFERENCES posts(id);

--
-- COMMENTS
--
SELECT
    c.commentid         "id",
    c.commentscore      score,
    c.postid            post_id,
    c.commenttext       "text",
    c.commentcreatedate creation_date,
    c.authorid          author_id
INTO "comments"
FROM comments_universal c;

ALTER TABLE "comments"
    ADD PRIMARY KEY ("id");

ALTER TABLE "comments"
    ADD FOREIGN KEY (post_id)
    REFERENCES posts ("id");

ALTER TABLE "comments"
    ADD FOREIGN KEY (author_id)
    REFERENCES authors ("id");

--
-- TAGS
--
SELECT DISTINCT
    UNNEST(string_to_array(p.tags, '::')) "name"
INTO tags
FROM posts_universal p
WHERE p.tags IS NOT NULL;

ALTER TABLE tags
    ADD PRIMARY KEY ("name");

--
-- POSTS_TAG
--
SELECT DISTINCT
    p."id"                                post_id,
    UNNEST(string_to_array(p.tags, '::')) "name"
INTO posts_tag
FROM posts_universal p
WHERE p.tags IS NOT NULL;

ALTER TABLE posts_tag
    ADD FOREIGN KEY ("name")
    REFERENCES tags ("name");

ALTER TABLE posts_tag
    ADD FOREIGN KEY (post_id)
    REFERENCES posts ("id");

--
-- USERS
--
CREATE TABLE users (
    "id"                SERIAL PRIMARY KEY,
    display_name        TEXT,
    creation_date       TIMESTAMP WITHOUT TIME ZONE,
    email               TEXT,
    "password"          TEXT,

    UNIQUE(email)
);

--
-- MARKED_POSTS_QUESTION
--
CREATE TABLE marked_posts_question (
    "user_id"           INTEGER REFERENCES users ("id"),
    posts_question_id   INTEGER REFERENCES posts_question ("id"),
    note                TEXT,

    UNIQUE ("user_id", posts_question_id)
);

--
-- MARKED_POSTS_ANSWER
--
CREATE TABLE marked_posts_answer (
    "user_id"           INTEGER REFERENCES users ("id"),
    posts_answer_id     INTEGER REFERENCES posts_answer ("id"),
    note                TEXT,

    UNIQUE ("user_id", posts_answer_id)
);
