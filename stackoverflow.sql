-- Create and insert into posts from posts_universal
select distinct on ("id")
    pu."id",
    pu.posttypeid type_id,
    pu.parentid parent_id,
    pu.acceptedanswerid accepted_answer_id,
    pu.creationdate creation_date,
    pu.score,
    pu.body,
    pu.closeddate closed_date,
    pu.title,
    pu.ownerid author_id
into posts
from posts_universal pu;

alter table posts
add primary key("id");

-- alter table posts
-- add column title_tokens TSVECTOR;

-- update posts p1
-- set title_tokens = to_tsvector(p1.title)
-- from posts p2;

-- Create pivot table link_posts
select distinct
    pu."id" post_id,
	pu.linkpostid link_id
into post_links
from posts_universal pu
where pu.linkpostid is not null
and pu.linkpostid not in (
    select linkpostid from posts_universal where linkpostid is not null
    except (select id from posts_universal)
);

alter table post_links
add foreign key (post_id) references posts(id);

alter table post_links
add foreign key (link_id) references posts(id);


-- Create table authors
create table authors (
    "id" int,
    display_name text,
    creation_date timestamp,
    "location" text,
    age int,

    primary key("id")
);

-- Insert unique owners into authors
insert into authors (
    "id",
    display_name,
    creation_date,
    "location",
    age
)
select distinct
    ownerid,
    ownerdisplayname,
    ownercreationdate,
    ownerlocation,
    ownerage
from posts_universal;


-- Insert unique authors into authors that are not there already
insert into authors(
    "id",
    display_name,
    creation_date,
    "location",
    age
)
select distinct
    authorid,
    authordisplayname,
    authorcreationdate,
    authorlocation,
    authorage
from comments_universal
where not exists (
    select * from authors
    where authors.id = authorid
);


-- Create comments table
select
    c.commentid "id",
    c.commentscore score,
    c.postid post_id,
    c.commenttext "text",
    c.commentcreatedate creation_date,
    c.authorid author_id
into "comments"
from comments_universal c;

alter table "comments"
add primary key("id");

alter table "comments"
add foreign key (author_id) references authors("id");


-- Create tags table
select distinct unnest(string_to_array(pu.tags, '::')) "name"
into tags
from posts_universal pu
where pu.tags is not null;

alter table tags
add primary key("name");


-- Create pivot table post_tags
select distinct
    unnest(string_to_array(pu.tags, '::')) "name",
    pu."id" post_id
into post_tags
from posts_universal pu
where pu.tags is not null;

alter table post_tags
add foreign key ("name") references tags("name");

alter table post_tags
add foreign key (post_id) references posts("id");


-- TODO - Author number is wrong
--
create table users(
    "id" serial primary key,
    display_name text,
    creation_date timestamp without time zone,
    email text,
    password text,
    deactivation_date timestamp without time zone
);

create table searches(
    "id" serial primary key,
    "user_id" integer references users(id),
    search_text text
);

create table favorite_posts(
    "user_id" integer not null references users(id),
    post_id integer not null references posts(id),
    note text,
    unique("user_id", post_id)
);

create table favorite_comments(
    "user_id" integer not null references users("id"),
    comment_id integer not null references comments("id"),
    note text,
    unique("user_id", comment_id)
);
