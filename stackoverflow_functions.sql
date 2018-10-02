-- Function for filtering by post tags
create or replace function filter_by_tags(_tags text[])
returns setof posts_with_tags as $$
    begin
        return query
            select posts.*, array_agg(post_tags.name) tags
            from posts
            join post_tags
            on posts.id = post_tags.post_id
            where post_tags.name = any(_tags)
            group by posts.id;
    end
$$ language plpgsql;

-- Function for filtering by words occurring in the post title and tags
create or replace function filter_by_words(_user_id integer = null, _search text = null, _tags text[] = null)
returns setof posts_with_tags as $$
    declare
        _query tsquery;
        _flag boolean;
    begin
        _query := plainto_tsquery('english', _search);
        _flag := (_search = '') is not false;

        if not _flag and _user_id is not null then
            perform add_to_search_history(_user_id, _search);
        end if;

        if _tags is not null then
            return query
                select * from filter_by_tags(_tags)
                where (_flag or title_tokens @@ _query);
        else
            return query
                select * from posts_with_tags
                where (_flag or title_tokens @@ _query);
        end if;
    end
$$ language plpgsql;

-- Adding to search history
create or replace function add_to_search_history(user_id integer, search_text text)
returns void as $$
begin
	insert into searches
	(user_id, search_text) 
	values 
	(user_id, search_text);
end;
$$
language plpgsql;

-- Favoriting functionality
create or replace function favorite_comment(user_id integer, comment_id integer, note text)
returns void as $$
begin
	insert into favorite_comments
	(user_id, comment_id, note) 
	values 
	(user_id, comment_id, note);
end;
$$
language plpgsql;

create or replace function favorite_post(user_id integer, post_id integer, note text)
returns void as $$
begin
	insert into favorite_posts(
	user_id, post_id, note) 
	values 
	(user_id, post_id, note);
end;
$$
language plpgsql;

-- User functionality
create or replace function register_user(display_name text, email text, password text)
returns void as $$
begin
	insert into users 
	(display_name, creation_date, email, password) 
	values 
	(display_name, now()::timestamp(0), email, password);
end;
$$
language plpgsql;

create or replace function get_user_by_email(mail text)
returns setof users as
$$
begin 
	return query select *
	from users
	where email=mail;
end;
$$ 
language plpgsql;

create or replace function get_deactivated_user_by_email(mail text)
returns setof users as
$$
begin 
	return query select *
	from deactivated_users
	where email=mail;
end;
$$ 
language plpgsql;


create or replace function deactivate_user(arg_user_id integer)
returns void as $$
begin
	-- Copy data into deactivated_users
	insert into deactivated_users (id, display_name, creation_date, email, password)
	select id, display_name, creation_date, email, password 
	from users where users.id = arg_user_id;

	--create deactivation timestamp
	update deactivated_users 
	set deactivation_date = now()::timestamp(0)
	where deactivated_users.id = arg_user_id;

	-- copy data into deactivated searches, comments and posts
	insert into deactivated_searches 
	select * from searches where searches.user_id = arg_user_id;
	insert into deactivated_favorite_comments
	select * from favorite_comments where favorite_comments.user_id = arg_user_id;
	insert into deactivated_favorite_posts
	select * from favorite_posts where favorite_posts.user_id = arg_user_id;

	-- delete data from original tables
	delete from searches where searches.user_id = arg_user_id;
	delete from favorite_comments where favorite_comments.user_id = arg_user_id;
	delete from favorite_posts where favorite_posts.user_id = arg_user_id;
	delete from users where users.id = arg_user_id;
end;
$$
language plpgsql;


create or replace function reactivate_user(arg_user_id integer)
returns void as $$
begin
	-- move deactivated user into active users
	insert into users (id, display_name, creation_date, email, password)
	select id, display_name, creation_date, email, password 
	from deactivated_users where deactivated_users.id = arg_user_id;

	-- copy deactivated data into active user data
	insert into searches 
	select * from deactivated_searches where deactivated_searches.user_id = arg_user_id;
	insert into favorite_comments
	select * from deactivated_favorite_comments where deactivated_favorite_comments.user_id = arg_user_id;
	insert into favorite_posts
	select * from deactivated_favorite_posts where deactivated_favorite_posts.user_id = arg_user_id;
	
	--delete deactivated data
	delete from deactivated_searches where deactivated_searches.user_id = arg_user_id;
	delete from deactivated_favorite_comments where deactivated_favorite_comments.user_id = arg_user_id;
	delete from deactivated_favorite_posts where deactivated_favorite_posts.user_id = arg_user_id;
	delete from deactivated_users where deactivated_users.id = arg_user_id;
end;
$$
language plpgsql;

create or replace function delete_users() 
returns void as
$$
begin
	delete from deactivated_users
	where deactivation_date < (now()::timestamp(0) - interval '14 days');
	
end;
$$ 
language plpgsql;

-- Get favorite comments

create or replace function get_favorite_comments(arg_user_id integer)
returns table(comment_id integer, score integer, post_id integer, "text" text, creation_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select comments.id, comments.score, comments.post_id, comments.text, comments.creation_date, comments.author_id, favorite_comments.note
	from comments 
	join favorite_comments on favorite_comments.comment_id = comments.id
	where favorite_comments.user_id = arg_user_id;
end;
$$ 
language plpgsql;

create or replace function get_favorite_deactivated_comments(arg_user_id integer)
returns table(comment_id integer, score integer, post_id integer, "text" text, creation_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select comments.id, comments.score, comments.post_id, comments.text, comments.creation_date, comments.author_id, deactivated_favorite_comments.note
	from comments 
	join deactivated_favorite_comments on deactivated_favorite_comments.comment_id = comments.id
	where deactivated_favorite_comments.user_id = arg_user_id;
end;
$$ 
language plpgsql;

-- Get favorite posts
create or replace function get_favorite_posts(arg_user_id integer)
returns table(post_id integer, "type" integer, answer_id integer, parent_id integer, score integer,title text, body text, creation_date timestamp without time zone, closed_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select posts.id, posts.type_id, posts.accepted_answer_id, posts.parent_id, posts.score, posts.title, posts.body, posts.creation_date, posts.closed_date, posts.author_id, favorite_posts.note
	from posts 
	join favorite_posts on favorite_posts.post_id = posts.id
	where favorite_posts.user_id = arg_user_id;
end;
$$ 
language plpgsql;

create or replace function get_favorite_deactivated_posts(arg_user_id integer)
returns table(post_id integer, "type" integer, answer_id integer, parent_id integer, score integer,title text, body text, creation_date timestamp without time zone, closed_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select posts.id, posts.type_id, posts.accepted_answer_id, posts.parent_id, posts.score, posts.title, posts.body, posts.creation_date, posts.closed_date, posts.author_id, deactivated_favorite_posts.note
	from posts 
	join deactivated_favorite_posts on deactivated_favorite_posts.post_id = posts.id
	where deactivated_favorite_posts.user_id = arg_user_id;
end;
$$ 
language plpgsql;

-- Function for filtering by post tags
create or replace function filter_by_tags(_tags text[])
returns setof posts_with_tags as $$
    begin
        return query
            select posts.*, array_agg(post_tags.name) tags
            from posts
            join post_tags
            on posts.id = post_tags.post_id
            where post_tags.name = any(_tags)
            group by posts.id;
    end
$$ language plpgsql;

-- Function for filtering by words occurring in the post title and tags
create or replace function filter_by_words(_user_id integer = null, _search text = null, _tags text[] = null)
returns setof posts_with_tags as $$
    declare
        _query tsquery;
        _flag boolean;
    begin
        _query := plainto_tsquery('english', _search);
        _flag := (_search = '') is not false;

        if not _flag and _user_id is not null then
            perform add_to_search_history(_user_id, _search);
        end if;

        if _tags is not null then
            return query
                select * from filter_by_tags(_tags)
                where (_flag or title_tokens @@ _query);
        else
            return query
                select * from posts_with_tags
                where (_flag or title_tokens @@ _query);
        end if;
    end
$$ language plpgsql;

-- Adding to search history
create or replace function add_to_search_history(user_id integer, search_text text)
returns void as $$
begin
	insert into searches
	(user_id, search_text) 
	values 
	(user_id, search_text);
end;
$$
language plpgsql;

-- Favoriting functionality
create or replace function favorite_comment(user_id integer, comment_id integer, note text)
returns void as $$
begin
	insert into favorite_comments
	(user_id, comment_id, note) 
	values 
	(user_id, comment_id, note);
end;
$$
language plpgsql;

create or replace function favorite_post(user_id integer, post_id integer, note text)
returns void as $$
begin
	insert into favorite_posts(
	user_id, post_id, note) 
	values 
	(user_id, post_id, note);
end;
$$
language plpgsql;

-- User functionality
create or replace function register_user(display_name text, email text, password text)
returns void as $$
begin
	insert into users 
	(display_name, creation_date, email, password) 
	values 
	(display_name, now()::timestamp(0), email, password);
end;
$$
language plpgsql;

create or replace function get_user_by_email(mail text)
returns setof users as
$$
begin 
	return query select *
	from users
	where email=mail;
end;
$$ 
language plpgsql;

create or replace function get_deactivated_user_by_email(mail text)
returns setof users as
$$
begin 
	return query select *
	from deactivated_users
	where email=mail;
end;
$$ 
language plpgsql;


create or replace function deactivate_user(arg_user_id integer)
returns void as $$
begin
	-- Copy data into deactivated_users
	insert into deactivated_users (id, display_name, creation_date, email, password)
	select id, display_name, creation_date, email, password 
	from users where users.id = arg_user_id;

	--create deactivation timestamp
	update deactivated_users 
	set deactivation_date = now()::timestamp(0)
	where deactivated_users.id = arg_user_id;

	-- copy data into deactivated searches, comments and posts
	insert into deactivated_searches 
	select * from searches where searches.user_id = arg_user_id;
	insert into deactivated_favorite_comments
	select * from favorite_comments where favorite_comments.user_id = arg_user_id;
	insert into deactivated_favorite_posts
	select * from favorite_posts where favorite_posts.user_id = arg_user_id;

	-- delete data from original tables
	delete from searches where searches.user_id = arg_user_id;
	delete from favorite_comments where favorite_comments.user_id = arg_user_id;
	delete from favorite_posts where favorite_posts.user_id = arg_user_id;
	delete from users where users.id = arg_user_id;
end;
$$
language plpgsql;


create or replace function reactivate_user(arg_user_id integer)
returns void as $$
begin
	-- move deactivated user into active users
	insert into users (id, display_name, creation_date, email, password)
	select id, display_name, creation_date, email, password 
	from deactivated_users where deactivated_users.id = arg_user_id;

	-- copy deactivated data into active user data
	insert into searches 
	select * from deactivated_searches where deactivated_searches.user_id = arg_user_id;
	insert into favorite_comments
	select * from deactivated_favorite_comments where deactivated_favorite_comments.user_id = arg_user_id;
	insert into favorite_posts
	select * from deactivated_favorite_posts where deactivated_favorite_posts.user_id = arg_user_id;
	
	--delete deactivated data
	delete from deactivated_searches where deactivated_searches.user_id = arg_user_id;
	delete from deactivated_favorite_comments where deactivated_favorite_comments.user_id = arg_user_id;
	delete from deactivated_favorite_posts where deactivated_favorite_posts.user_id = arg_user_id;
	delete from deactivated_users where deactivated_users.id = arg_user_id;
end;
$$
language plpgsql;


create or replace function delete_users() 
returns void as
$$
begin
	delete from deactivated_users
	where deactivation_date < (now()::timestamp(0) - interval '14 days');
	
end;
$$ 
language plpgsql;

-- Get favorite comments
create or replace function get_favorite_comments(arg_user_id integer)
returns table(comment_id integer, score integer, post_id integer, "text" text, creation_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select comments.id, comments.score, comments.post_id, comments.text, comments.creation_date, comments.author_id, favorite_comments.note
	from comments 
	join favorite_comments on favorite_comments.comment_id = comments.id
	where favorite_comments.user_id = arg_user_id;
end;
$$ 
language plpgsql;

create or replace function get_favorite_deactivated_comments(arg_user_id integer)
returns table(comment_id integer, score integer, post_id integer, "text" text, creation_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select comments.id, comments.score, comments.post_id, comments.text, comments.creation_date, comments.author_id, deactivated_favorite_comments.note
	from comments 
	join deactivated_favorite_comments on deactivated_favorite_comments.comment_id = comments.id
	where deactivated_favorite_comments.user_id = arg_user_id;
end;
$$ 
language plpgsql;

-- get favorite posts
create or replace function get_favorite_posts(arg_user_id integer)
returns table(post_id integer, "type" integer, answer_id integer, parent_id integer, score integer,title text, body text, creation_date timestamp without time zone, closed_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select posts.id, posts.type_id, posts.accepted_answer_id, posts.parent_id, posts.score, posts.title, posts.body, posts.creation_date, posts.closed_date, posts.author_id, favorite_posts.note
	from posts 
	join favorite_posts on favorite_posts.post_id = posts.id
	where favorite_posts.user_id = arg_user_id;
end;
$$ 
language plpgsql;

create or replace function get_favorite_deactivated_posts(arg_user_id integer)
returns table(post_id integer, "type" integer, answer_id integer, parent_id integer, score integer,title text, body text, creation_date timestamp without time zone, closed_date timestamp without time zone, author_id integer, note text) as
$$
begin 
	return query 
	select posts.id, posts.type_id, posts.accepted_answer_id, posts.parent_id, posts.score, posts.title, posts.body, posts.creation_date, posts.closed_date, posts.author_id, deactivated_favorite_posts.note
	from posts 
	join deactivated_favorite_posts on deactivated_favorite_posts.post_id = posts.id
	where deactivated_favorite_posts.user_id = arg_user_id;
end;
$$ 
language plpgsql;


--- Get all the posts with  links

create or replace function get_posts_with_links(postid integer)
returns setof posts_with_tags as $$
    begin
       return query
            select distinct posts.*, array_agg(post_tags.name) tags
            from posts
            join post_links on posts.id = post_links.post_id
            join post_tags on posts.id = post_tags.post_id
            where post_links.link_id is not null
		and posts.id=postid
            group by posts.id;
    end
$$ language plpgsql;

--- Get the users history
create or replace function get_users_search_history(userid integer)
returns setof searches as $$
	begin
	   return query
		select searches.*
		from searches 
			join users on searches.user_id=users.id
		where users.id=userid
		group by searches.id,users.id;
	end
$$ language plpgsql;