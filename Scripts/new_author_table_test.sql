--
-- CREATE AUTHORS TABLE
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
