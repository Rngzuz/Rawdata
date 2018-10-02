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

create or replace function get_user_by_email( mail text)
returns setof users as
$$
begin 
	return query select *
	from users
	where email=mail;
end;
$$ 
language plpgsql;


create or replace function deactivate_user(arg_user_id integer)
returns void as $$
begin
	insert into deactivated_users (id, display_name, creation_date, email, password)
	select id, display_name, creation_date, email, password 
	from users where users.id = arg_user_id;

	update deactivated_users 
	set deactivation_date = now()::timestamp(0)
	where deactivated_users.id = arg_user_id;

	insert into deactivated_searches 
	select * from searches where searches.user_id = arg_user_id;
	insert into deactivated_favorite_comments
	select * from favorite_comments where favorite_comments.user_id = arg_user_id;
	insert into deactivated_favorite_posts
	select * from favorite_posts where favorite_posts.user_id = arg_user_id;

	
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
	insert into users (id, display_name, creation_date, email, password)
	select id, display_name, creation_date, email, password 
	from deactivated_users where deactivated_users.id = arg_user_id;

	insert into searches 
	select * from deactivated_searches where deactivated_searches.user_id = arg_user_id;
	insert into favorite_comments
	select * from deactivated_favorite_comments where deactivated_favorite_comments.user_id = arg_user_id;
	insert into favorite_posts
	select * from deactivated_favorite_posts where deactivated_favorite_posts.user_id = arg_user_id;

	
	delete from deactivated_searches where deactivated_searches.user_id = arg_user_id;
	delete from deactivated_favorite_comments where deactivated_favorite_comments.user_id = arg_user_id;
	delete from deactivated_favorite_posts where deactivated_favorite_posts.user_id = arg_user_id;
	delete from deactivated_users where deactivated_users.id = arg_user_id;
end;
$$
language plpgsql;
