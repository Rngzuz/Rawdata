drop function if exists filter_by_word;
drop function if exists filter_by_tags;
drop view if exists posts_with_tags;
drop table if exists deactivated_favorite_posts;
drop table if exists deactivated_favorite_comments;
drop table if exists deactivated_searches;
drop table if exists deactivated_users;
drop table if exists favorite_comments;
drop table if exists favorite_posts;
drop table if exists searches;
drop table if exists users;
drop table if exists post_links;
drop table if exists post_tags;
drop table if exists comments;
drop table if exists posts;
drop table if exists tags;
drop table if exists authors;

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

-- alter table comments
-- add column text_tokens TSVECTOR;

-- update comments c1
-- set text_tokens = to_tsvector(c1.text)
-- from comments c2;


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

-- Create users table
create table users(
    "id" serial primary key,
    display_name text,
    creation_date timestamp without time zone,
    email text,
    "password" text,
    deactivation_date timestamp without time zone
);

create table deactivated_users (
	id serial primary key,
	display_name text,
	creation_date timestamp without time zone,
	email text,
	"password" text,
	deactivation_date timestamp without time zone,
	unique (email)
);

-- Create searches table
create table searches(
    "id" serial primary key,
    "user_id" integer references users(id),
    search_text text
);

create table deactivated_searches(
		id serial primary key,
		user_id integer references deactivated_users(id) ON DELETE CASCADE,
		search_text text)
;

-- Create favorite_posts table
create table favorite_posts(
    "user_id" integer not null references users(id),
    post_id integer not null references posts(id),
    note text,
    unique("user_id", post_id)
);

create table deactivated_favorite_posts(
		user_id integer not null references deactivated_users(id) ON DELETE CASCADE,
		post_id integer not null references posts(id),
		note text,
		unique(user_id, post_id)
);

-- Create favorite_comments table
create table favorite_comments(
    "user_id" integer not null references users("id"),
    comment_id integer not null references comments("id"),
    note text,
    unique("user_id", comment_id)
);

create table deactivated_favorite_comments(
		user_id integer not null references deactivated_users(id) ON DELETE CASCADE,
		comment_id integer not null references comments(id),
		note text,
		unique(user_id, comment_id)
);

-- View which include post tags
create view posts_with_tags as
select posts.*, array_agg(post_tags.name) tags
from posts
join post_tags
on posts.id = post_tags.post_id
group by posts.id;
