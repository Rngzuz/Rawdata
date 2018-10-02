-- View which include post tags
create view posts_with_tags as
select posts.*, array_agg(post_tags.name) tags
from posts
join post_tags
on posts.id = post_tags.post_id
group by posts.id;

-- Function for filtering by post tags
create function filter_by_tags(_tags text[])
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
create function filter_by_words(_user_id integer, _search text = null, _tags text[] = null)
returns setof posts_with_tags as $$
    declare
        _search_query tsquery,
        _flag boolean;
    begin
        _search_query := plainto_tsquery('english', _search);
        _flag := (_search = '') is not false;

        if _flag then

        end if;

        if _tags is not null then
            return query
                select * from filter_by_tags(_tags)
                where (_flag or title_tokens @@ _search_query);
        else
            return query
                select * from posts_with_tags
                where (_flag or title_tokens @@ _search_query);
        end if;
    end
$$ language plpgsql;

-- Test queries
select * from filter_by_words(
    'testing unit',
    array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]
);

select * from filter_by_words('testing unit');

select * from filter_by_tags(
    array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]
);
