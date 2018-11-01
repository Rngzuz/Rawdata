
DROP FUNCTION IF EXISTS
    get_posts_by_tags,
    get_all_posts,
    get_all_marked_posts,
    get_all_comments,
    get_all_marked_comments,
    add_user,
    toggle_marked_post,
    toggle_marked_comment,
    strip_tags
    CASCADE;

CREATE FUNCTION get_posts_by_tags(_tags TEXT[])
RETURNS SETOF posts_with_tags AS $$
    BEGIN
        RETURN QUERY
            SELECT posts.*, ARRAY_AGG(posts_tags.name) tags
            FROM posts
            JOIN posts_tags
            ON posts.id = posts_tags.post_id
            WHERE posts_tags.name = ANY(_tags)
            GROUP BY posts.id;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION get_all_posts(_search TEXT = NULL, _tags TEXT[] = NULL, _anwered_only BOOLEAN = FALSE, _user_id INTEGER = NULL)
RETURNS SETOF posts_with_tags AS $$
    DECLARE
        _query TSQUERY;
        _flag BOOLEAN;
    BEGIN
        -- convert _search to ts query (will compare each word by AND)
        _query := PLAINTO_TSQUERY('english', _search);

        -- check if _search is empty or null
        _flag := (_search = '') IS NOT FALSE;

        -- save to search history if _user_id is supplied
        IF NOT _flag AND _user_id IS NOT NULL THEN
	        INSERT INTO searches ("user_id", search_text) VALUES (_user_id, _search) ON CONFLICT DO NOTHING;
        END IF;

        -- if any tags are supplied then filter and search
        -- else only search for the _search string in the title column
        IF _tags IS NOT NULL THEN
            RETURN QUERY
                SELECT * FROM get_posts_by_tags(_tags)
                WHERE ((_flag OR title_tokens @@ _query) OR (_flag OR body_tokens @@ _query)) -- if _flag is true then do not search in either the title or body
                AND (NOT _anwered_only OR accepted_answer_id IS NOT NULL); -- if _anwered_only is false then do not filter by accepted_answer_id else if true then only fetch answered posts
        ELSE
            RETURN QUERY
                SELECT * FROM posts_with_tags
                WHERE ((_flag OR title_tokens @@ _query) OR (_flag OR body_tokens @@ _query))
                AND (NOT _anwered_only OR accepted_answer_id IS NOT NULL);
        END IF;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION get_all_marked_posts(_search TEXT = NULL, _tags TEXT[] = NULL, _anwered_only BOOLEAN = FALSE, _user_id INTEGER = NULL)
RETURNS SETOF posts_with_tags AS $$
    BEGIN
        IF _user_id IS NOT NULL THEN
            IF _user_id IN (SELECT "id" FROM users) THEN
                RETURN QUERY
                    SELECT * FROM get_all_posts(_search, _tags, _anwered_only, _user_id) p
                    WHERE p."id" IN (SELECT post_id FROM marked_posts WHERE "user_id" = _user_id);
            ELSE
                RAISE EXCEPTION 'search_marked_posts: User does not exist.';
            END IF;
        ELSE
            RAISE EXCEPTION 'search_marked_posts: User ID required.';
        END IF;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION get_all_comments(_search TEXT = NULL, _user_id INTEGER = NULL)
RETURNS SETOF comments AS $$
    DECLARE
        _query TSQUERY;
        _flag BOOLEAN;
    BEGIN
        -- convert _search to ts query (will compare each word by AND)
        _query := PLAINTO_TSQUERY('english', _search);

        -- check if _search is empty or null
        _flag := (_search = '') IS NOT FALSE;

        -- save to search history if _user_id is supplied
        IF NOT _flag AND _user_id IS NOT NULL THEN
	        INSERT INTO searches ("user_id", search_text) VALUES (_user_id, _search) ON CONFLICT DO NOTHING;
        END IF;

        RETURN QUERY
            SELECT * FROM comments
            WHERE (_flag OR text_tokens @@ _query);
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION get_all_marked_comments(_search TEXT = NULL, _user_id INTEGER = NULL)
RETURNS SETOF comments AS $$
    BEGIN
        IF _user_id IS NOT NULL THEN
            IF _user_id IN (SELECT "id" FROM users) THEN
                RETURN QUERY
                    SELECT * FROM get_all_comments(_search, _user_id) c
                    WHERE c."id" IN (SELECT comment_id FROM marked_comments WHERE "user_id" = _user_id);
            ELSE
                RAISE EXCEPTION 'search_marked_comments: User does not exist.';
            END IF;
        ELSE
            RAISE EXCEPTION 'search_marked_comments: User ID required.';
        END IF;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION add_user(_display_name TEXT, _email TEXT, _password TEXT)
RETURNS VOID AS $$
BEGIN
	INSERT INTO users (display_name, creation_date, email, "password")
	VALUES (_display_name, NOW()::TIMESTAMP(0), _email, _password);
END;
$$
LANGUAGE plpgsql;

CREATE FUNCTION toggle_marked_post(_user_id INTEGER, _comment_id INTEGER, _note TEXT = NULL)
RETURNS VOID AS $$
    BEGIN
        IF NOT EXISTS (SELECT * FROM marked_posts WHERE "user_id" = _user_id AND post_id = _post_id) THEN
            INSERT INTO marked_posts VALUES(_user_id, _post_id, _note);
        ELSE
            DELETE FROM marked_posts WHERE "user_id" = _user_id AND post_id = _post_id;
        END IF;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION toggle_marked_comment(_user_id INTEGER, _comment_id INTEGER, _note TEXT = NULL)
RETURNS VOID AS $$
    BEGIN
        IF NOT EXISTS (SELECT * FROM marked_comments WHERE "user_id" = _user_id AND comment_id = _comment_id) THEN
            INSERT INTO marked_comments VALUES(_user_id, _comment_id, _note);
        ELSE
            DELETE FROM marked_comments WHERE "user_id" = _user_id AND comment_id = _comment_id;
        END IF;
    END
$$ LANGUAGE plpgsql;


-- ADD THIS BEFORE SCHEMA AND FUNCTIONS
CREATE FUNCTION strip_tags(TEXT) RETURNS TEXT AS $$
    SELECT regexp_replace(regexp_replace($1, E'(?x)<[^>]*?(\s alt \s* = \s* ([\'"]) ([^>]*?) \2) [^>]*? >', E'\3'), E'(?x)(< [^>]*? >)', '', 'g')
$$ LANGUAGE SQL;
