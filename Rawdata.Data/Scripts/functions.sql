DROP FUNCTION IF EXISTS
    add_user,
    toggle_marked_post,
    toggle_marked_comment,
    exact_match,
    best_match,
    ranked_weighted_match,
    word_to_word,
    post_word_association,
    generate_force_graph_input
    CASCADE;

--
-- add_user
--
CREATE FUNCTION add_user(_display_name TEXT, _email TEXT, _password TEXT)
RETURNS SETOF users AS $$
BEGIN
	INSERT INTO users (display_name, creation_date, email, "password")
	VALUES (_display_name, NOW()::TIMESTAMP(0), _email, _password);

    RETURN QUERY SELECT * FROM users WHERE email = _email;
END;
$$
LANGUAGE plpgsql;

--
-- toggle_marked_post
--
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

--
-- toggle_marked_comment
--
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

--
-- word excerpt
--

CREATE OR REPLACE FUNCTION word_excerpt(_words TEXT[])
RETURNS TABLE (post_id int, sentences text[]) AS $$
DECLARE
	_word TEXT;
	_sub_queries TEXT[];
	_query TEXT := 'select post_id, ARRAY_AGG(sentence order by sentence) from (select post_id, array_to_string(ARRAY_AGG(word order by sen, idx), '' '') sentence from';
BEGIN
	FOREACH _word IN array _words LOOP
		_sub_queries := array_append(
			_sub_queries,
			'(select distinct post_id, sentence from post_word_index where word = ''' || _word || ''')'
	        );
	END LOOP;

	_query := _query
		|| ' ((' || array_to_string(_sub_queries, ' union distinct ') || ')'
		|| 't_union join (select id,sen, idx, word from words where what = ''body'') words on t_union.post_id = words.id and t_union.sentence = words.sen) t_joined
	group by post_id, sen) t_sentences group by post_id';

	RAISE NOTICE '%', _query;
	RETURN QUERY EXECUTE _query;
END
$$ LANGUAGE 'plpgsql';


--
-- exact_match
--
CREATE OR REPLACE FUNCTION exact_match(_words TEXT[])
RETURNS TABLE (post_id INT, "rank" FLOAT) AS $$
DECLARE
   _word TEXT;
   _sub_queries TEXT[];
   _query TEXT := 'select posts.id, cast(score as float) rank from posts, ';
   _count INT := 0;
BEGIN
   FOREACH _word IN array _words LOOP
       _sub_queries := array_append(
           _sub_queries,
           '(select post_id from post_word_index where word = ''' || _word || ''') t' || _count
       );

       _count := _count + 1;
   END LOOP;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' join ') || ' using (post_id))'
       || ' t_all where t_all.post_id = posts.id group by posts.id order by score desc';

   RAISE NOTICE '%', _query;
   RETURN QUERY EXECUTE _query;
END
$$ LANGUAGE 'plpgsql';

--
-- exact_match_context
--
CREATE OR REPLACE FUNCTION exact_match_context(_words TEXT[])
RETURNS TABLE (post_id INT, "rank" FLOAT, sentences text[]) AS $$
BEGIN

   RETURN QUERY
   SELECT rank.post_id, rank.rank, excerpt.sentences FROM (
	(SELECT * FROM word_excerpt(_words)) excerpt 
	JOIN
	(SELECT * FROM exact_match(_words)) rank
	ON excerpt.post_id = rank.post_id
   ) order by rank desc;
END
$$ LANGUAGE 'plpgsql';

--
-- best_match
-- 
CREATE OR REPLACE FUNCTION best_match(_words TEXT[])
RETURNS TABLE (post_id INT, "rank" FLOAT) AS $$
DECLARE
   _word TEXT;
   _sub_queries TEXT[];
   _query TEXT := 'select posts.id, cast(sum(t_all.score) as float) rank from posts, ';
BEGIN
   FOREACH _word IN array _words LOOP
       _sub_queries := array_append(
           _sub_queries,
           '(select distinct post_id, 1 score from post_word_index where word = ''' || _word || ''')'
       );
   END LOOP;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' union all ') || ')'
       || ' t_all where t_all.post_id = posts.id group by posts.id order by rank desc, posts.score desc';

   RAISE NOTICE '%', _query;
   RETURN QUERY EXECUTE _query;
END
$$ LANGUAGE 'plpgsql';

--
-- best_match_context
--
CREATE OR REPLACE FUNCTION best_match_context(_words TEXT[])
RETURNS TABLE (post_id INT, "rank" FLOAT, sentences text[]) AS $$
BEGIN

   RETURN QUERY
   SELECT rank.post_id, rank.rank, excerpt.sentences FROM (
	(SELECT * FROM word_excerpt(_words)) excerpt 
	JOIN
	(SELECT * FROM best_match(_words)) rank
	ON excerpt.post_id = rank.post_id
   ) order by rank desc;
END
$$ LANGUAGE 'plpgsql';

--
-- ranked_weighted_match
--
CREATE OR REPLACE FUNCTION ranked_weighted_match(_words TEXT[])
RETURNS TABLE (post_id INT, "rank" FLOAT) AS $$
DECLARE
   _word TEXT;
   _sub_queries TEXT[];
   _query TEXT := 'select posts.id, sum(t_all.tf_idf) rank from posts, ';
BEGIN
   FOREACH _word IN array _words LOOP
       _sub_queries := array_append(
           _sub_queries,
           '(select distinct post_id, tf_idf from post_word_index where word = ''' || _word || ''')'
       );
   END LOOP;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' union all ') || ')'
       || ' t_all where t_all.post_id = posts.id group by posts.id order by rank desc, posts.score desc';

   RAISE NOTICE '%', _query;
   RETURN QUERY EXECUTE _query;
END
$$ LANGUAGE 'plpgsql';


--
-- ranked_weighted_match_context
--
CREATE OR REPLACE FUNCTION ranked_weighted_match_context(_words TEXT[])
RETURNS TABLE (post_id INT, "rank" FLOAT, sentences text[]) AS $$
BEGIN

   RETURN QUERY
   SELECT rank.post_id, rank.rank, excerpt.sentences FROM (
	(SELECT * FROM word_excerpt(_words)) excerpt 
	JOIN
	(SELECT * FROM ranked_weighted_match(_words)) rank
	ON excerpt.post_id = rank.post_id
   ) order by rank desc;
END
$$ LANGUAGE 'plpgsql';

--
-- word_to_word
--
CREATE OR REPLACE FUNCTION word_to_word(_word TEXT)
RETURNS TABLE(word TEXT, freq BIGINT) AS $$
BEGIN
    RETURN query
      SELECT
        t1.word,
        COUNT(*) AS freq
      FROM
        post_word_index t1
      LEFT JOIN post_word_index t2 ON t1.post_id = t2.post_id
      WHERE  t2.word = lower(_word) AND t1.word != lower(_word) -- check so that we do not return the query as
      GROUP BY t1.word
      ORDER BY freq DESC;
END
$$ LANGUAGE 'plpgsql';

--
--post_word_association
--
CREATE TABLE post_word_association AS
    SELECT w1.word word1, w2.word word2, COUNT(*) AS grade
    FROM post_word_index w1, post_word_index w2
    LEFT JOIN post_word_count pf
        USING (word)
    WHERE w1.post_id = w2.post_id AND w1.word < w2.word
    AND w1.tf_idf > 0.00002 AND w2.tf_idf > 0.00002 AND pf.count > 20
    GROUP BY w1.word, w2.word ORDER BY grade DESC;

CREATE INDEX pwa_word1_index ON post_word_association (word1);
CREATE INDEX pwa_word2_index ON post_word_association (word2);

CREATE OR REPLACE FUNCTION get_word_association(_word TEXT)
RETURNS SETOF post_word_association AS $$
BEGIN
    RETURN query
        SELECT * FROM post_word_association WHERE word1 = LOWER(_word)
        UNION
        SELECT * FROM post_word_association WHERE word2 = LOWER(_word)
        ORDER BY grade DESC;
END
$$ LANGUAGE 'plpgsql';


--
--post_word_association
--
CREATE OR REPLACE FUNCTION generate_force_graph_input(IN w VARCHAR(100), n REAL)
RETURNS TABLE (line text) AS $$
DECLARE
	l TEXT;
BEGIN
    LINE := '{"nodes":[';
    RETURN NEXT;

    LINE := '';
    RETURN NEXT;

    FOR l IN (SELECT '{"id":"'||lower(word2)||'"},' FROM post_word_association WHERE word1=w AND grade >= n) LOOP
        LINE:=l;
        RETURN NEXT;
    END LOOP;

    LINE := '{"id":"'||w||'"},';
    RETURN NEXT;

    LINE :=  '],';
    RETURN NEXT;

    LINE :=  '"links":[';
    RETURN NEXT;

    FOR l IN (
        SELECT '{"source":"'||lower(word1)||'", "target":"'||lower(word2)||'", "value":'||grade/2||'},'  FROM (
            SELECT * FROM post_word_association WHERE word1 = w AND grade >= n
            UNION
            SELECT * FROM post_word_association
            WHERE word1 IN (SELECT word2 FROM post_word_association WHERE word1 = w AND grade >= n)
            AND word2 IN (SELECT word2 FROM post_word_association WHERE word1 = w AND grade >= n)
        ) t
    ) LOOP
        LINE:=l;
        RETURN NEXT;
    END LOOP;

    LINE := ']}';
    RETURN NEXT;

    RETURN;
END;
$$
LANGUAGE 'plpgsql';