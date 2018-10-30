--
-- authors
--
SELECT
    t1.ownerid "id",
    t1.ownerdisplayname display_name,
    t1.ownercreationdate creation_date,
    t1.ownerlocation "location",
    t1.ownerage age
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

ALTER TABLE authors ADD PRIMARY KEY ("id");

INSERT INTO authors (
    "id",
    display_name,
    creation_date,
    "location",
    age
)
SELECT
    t2.authorid "id",
    t2.authordisplayname display_name,
    t2.authorcreationdate creation_date,
    t2.authorlocation "location",
    t2.authorage age
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
-- posts
--
SELECT DISTINCT ON ("id")
    p."id",
    (CASE WHEN (p.posttypeid = 1) THEN 'Question' ELSE 'Answer' END) discriminator,
    p.parentid parent_id,
    p.acceptedanswerid accepted_answer_id,
    p.creationdate creation_date,
    p.score,
    p.body,
    p.closeddate closed_date,
    p.title,
    to_tsvector(p.title) title_tokens,
    p.ownerid author_id,
    p.linkpostid link_id
INTO posts
FROM posts_universal p;

UPDATE posts SET accepted_answer_id = null WHERE accepted_answer_id NOT IN (SELECT "id" FROM posts);
UPDATE posts SET link_id = null WHERE link_id NOT IN (SELECT "id" FROM posts);

ALTER TABLE posts
    ADD PRIMARY KEY ("id"),
    ADD FOREIGN KEY (parent_id) REFERENCES posts ("id") ON DELETE CASCADE,
    ADD FOREIGN KEY (accepted_answer_id) REFERENCES posts ("id") ON DELETE SET NULL,
    ADD FOREIGN KEY (author_id) REFERENCES authors ("id") ON DELETE SET NULL,
    ADD FOREIGN KEY (link_id) REFERENCES posts ("id") ON DELETE SET NULL;

--
-- comments
--
SELECT
    c.commentid "id",
    c.commentscore score,
    c.postid post_id,
    c.commenttext "text",
    c.commentcreatedate creation_date,
    c.authorid author_id
INTO "comments"
FROM comments_universal c;

ALTER TABLE "comments"
    ADD PRIMARY KEY ("id"),
    ADD FOREIGN KEY (post_id) REFERENCES posts ("id"),
    ADD FOREIGN KEY (author_id) REFERENCES authors ("id");

--
-- tags
--
SELECT DISTINCT
    UNNEST(string_to_array(p.tags, '::')) "name"
INTO tags
FROM posts_universal p
WHERE p.tags IS NOT NULL;

ALTER TABLE tags ADD PRIMARY KEY ("name");

--
-- posts_tags
--
SELECT DISTINCT
    p."id" post_id,
    UNNEST(string_to_array(p.tags, '::')) "name"
INTO posts_tags
FROM posts_universal p
WHERE p.tags IS NOT NULL;

ALTER TABLE posts_tags
    ADD FOREIGN KEY ("name") REFERENCES tags ("name"),
    ADD FOREIGN KEY (post_id) REFERENCES posts ("id");

--
-- users
--
CREATE TABLE users (
    "id" SERIAL PRIMARY KEY,
    display_name TEXT,
    creation_date TIMESTAMP WITHOUT TIME ZONE,
    email TEXT,
    "password" TEXT,

    UNIQUE(email)
);

--
-- marked_posts
--
CREATE TABLE marked_posts (
    "user_id" INTEGER REFERENCES users ("id"),
    post_id INTEGER REFERENCES posts ("id"),
    note TEXT,

    UNIQUE ("user_id", post_id)
);
