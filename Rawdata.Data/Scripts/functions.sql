DROP FUNCTION IF EXISTS
    query_questions_by_tags,
    query_questions_by_text,
    query_questions,
    add_user,
    toggle_marked_post,
    toggle_marked_comment,
    strip_tags
    CASCADE;

CREATE FUNCTION query_questions_by_tags(_tags TEXT[])
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


CREATE FUNCTION query_questions_by_text(_search TEXT = NULL)
RETURNS SETOF posts_with_tags_and_marked AS $$
    DECLARE
        _query TSQUERY;
        _flag BOOLEAN;
    BEGIN
        _query := PLAINTO_TSQUERY('english', _search);
        _flag := (_search = '') IS NOT FALSE;

        RETURN QUERY
            SELECT * FROM posts_with_tags_and_marked
            WHERE (
                ((_flag OR title_tokens @@ _query) OR (_flag OR body_tokens @@ _query))
                OR "id" IN (SELECT "id" FROM posts WHERE type_id = 2  AND (_flag OR body_tokens @@ _query))
                OR "id" IN (SELECT "post_id" FROM comments WHERE (_flag OR text_tokens @@ _query))
            );
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION query_questions(_search TEXT = NULL, _tags TEXT[] = NULL, _anwered_only BOOLEAN = FALSE, _user_id INTEGER = NULL)
RETURNS SETOF posts_with_tags_and_marked AS $$
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
                SELECT * FROM query_questions_by_tags(_tags) t1
                JOIN query_questions_by_text(_search) t2
                ON t1."id" = t2."id"
                WHERE (NOT _anwered_only OR accepted_answer_id IS NOT NULL);
        ELSE
            RETURN QUERY
                SELECT * FROM query_questions_by_text(_search)
                WHERE (NOT _anwered_only OR accepted_answer_id IS NOT NULL);
        END IF;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION add_user(_display_name TEXT, _email TEXT, _password TEXT)
RETURNS SETOF users AS $$
BEGIN
	INSERT INTO users (display_name, creation_date, email, "password")
	VALUES (_display_name, NOW()::TIMESTAMP(0), _email, _password);

    RETURN QUERY SELECT * FROM users WHERE email = _email;
END;
$$
LANGUAGE plpgsql;

CREATE FUNCTION toggle_marked_post(_user_id INTEGER, _post_id INTEGER, _note TEXT = NULL)
RETURNS SETOF marked_posts AS $$
    BEGIN
		IF _user_id IS NULL THEN RAISE EXCEPTION 'toggle_marked_post: User ID required.';
		ELSIF _user_id NOT IN (SELECT "id" FROM users) THEN RAISE EXCEPTION 'toggle_marked_post: User does not exist.';
		ELSIF _post_id IS NULL THEN RAISE EXCEPTION 'toggle_marked_post: Post ID required.';
		ELSIF _post_id NOT IN (SELECT "id" FROM posts) THEN RAISE EXCEPTION 'toggle_marked_post: Post does not exist.';
		ELSIF NOT EXISTS (SELECT * FROM marked_posts WHERE "user_id" = _user_id AND post_id = _post_id) THEN
			INSERT INTO marked_posts VALUES(_user_id, _post_id, _note);
		ELSE 
			DELETE FROM marked_posts WHERE "user_id" = _user_id AND post_id = _post_id;
		END IF;

		RETURN QUERY SELECT * FROM marked_posts WHERE "user_id" = _user_id AND post_id = _post_id;
    END
$$ LANGUAGE plpgsql;

CREATE FUNCTION toggle_marked_comment(_user_id INTEGER, _comment_id INTEGER, _note TEXT = NULL)
RETURNS SETOF marked_comments AS $$
	BEGIN
			IF _user_id IS NULL THEN RAISE EXCEPTION 'toggle_marked_comment: User ID required.';
			ELSIF _user_id NOT IN (SELECT "id" FROM users) THEN RAISE EXCEPTION 'toggle_marked_comment: User does not exist.';
			ELSIF _comment_id IS NULL THEN RAISE EXCEPTION 'toggle_marked_comment: Comment ID required.';
			ELSIF _comment_id NOT IN (SELECT "id" FROM comments) THEN RAISE EXCEPTION 'toggle_marked_comment: Comment does not exist.';
			ELSIF NOT EXISTS (SELECT * FROM marked_comments WHERE "user_id" = _user_id AND comment_id = _comment_id) THEN
				INSERT INTO marked_comments VALUES(_user_id, _comment_id, _note);
			ELSE
				DELETE FROM marked_comments WHERE "user_id" = _user_id AND comment_id = _comment_id;
			END IF;

			RETURN QUERY SELECT * FROM marked_comments WHERE "user_id" = _user_id AND comment_id = _comment_id;
    END
$$ LANGUAGE plpgsql;


-- ADD THIS BEFORE SCHEMA AND FUNCTIONS
CREATE FUNCTION strip_tags(TEXT) RETURNS TEXT AS $$
    SELECT regexp_replace(regexp_replace($1, E'(?x)<[^>]*?(\s alt \s* = \s* ([\'"]) ([^>]*?) \2) [^>]*? >', E'\3'), E'(?x)(< [^>]*? >)', '', 'g')
$$ LANGUAGE SQL;
