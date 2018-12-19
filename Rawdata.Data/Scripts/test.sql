-- can be used as return type
CREATE TYPE match_result AS (
    post_id INT,
    "rank" FLOAT
);

--
-- exact_match
--
CREATE OR REPLACE FUNCTION exact_match(_words TEXT[])
RETURNS SETOF match_result AS
$$
BEGIN
    RETURN QUERY
        SELECT
            post_id,
            NULL::FLOAT "rank"
        FROM (
            SELECT post_id, ARRAY_AGG(word) words
            FROM post_word_index
            WHERE word = ANY(_words)
            GROUP BY post_id
        ) pwi -- group words by post id into an array
        WHERE words <@ _words; -- check if words array is contained in _words array
END
$$
LANGUAGE 'plpgsql';

    -- APPROACH 1: exact_match
    SELECT * FROM
        (SELECT post_id FROM post_word_index WHERE word = 'unit') t1,
        (SELECT post_id FROM post_word_index WHERE word = 'test') t2,
        (SELECT post_id FROM post_word_index WHERE word = 'sql') t3
    WHERE t1.post_id = t2.post_id
        AND t2.post_id = t3.post_id;

    -- APPROACH 2: exact_match
    SELECT *
    FROM (SELECT post_id FROM post_word_index WHERE word = 'unit') t1
    JOIN (SELECT post_id FROM post_word_index WHERE word = 'test') t2
        ON t1.post_id = t2.post_id
    JOIN (SELECT post_id FROM post_word_index WHERE word = 'sql') t3
        ON t1.post_id = t3.post_id

    -- APPROACH 3: exact_match
    SELECT
        post_id
    FROM
        (SELECT post_id, ARRAY_AGG(word) words
        FROM post_word_index
        WHERE word = ANY('{unit,test,sql}'::TEXT[])
        GROUP BY post_id) agg
    WHERE words @> '{unit,test,sql}'::TEXT[];



--
-- best_match
--
CREATE OR REPLACE FUNCTION best_match(_words TEXT[])
RETURNS SETOF match_result AS
$$
BEGIN
    RETURN QUERY
        SELECT
            post_id,
            SUM(CAST(score AS FLOAT)) "rank"
        FROM get_best_match_index(_words)
        GROUP BY post_id
        ORDER BY "rank" DESC;
END
$$
LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION get_best_match_index(_words TEXT[])
RETURNS TABLE (post_id INT, score INT) AS
$$
DECLARE
    _word TEXT;
BEGIN
    FOREACH _word IN ARRAY _words LOOP
        RETURN QUERY
            SELECT DISTINCT
                pwi.post_id,
                1 score
            FROM post_word_index pwi
            WHERE word = _word;
    END LOOP;
END
$$
LANGUAGE 'plpgsql';

    -- Was x4-5 times slower because of the temp table:
    -- CREATE OR REPLACE FUNCTION test_best_match(_words TEXT[])
    -- RETURNS SETOF match_result AS
    -- $$
    -- DECLARE
    --     _word TEXT;
    -- BEGIN
    --     CREATE TEMP TABLE t1 (
    --         post_id INT,
    --         score INT
    --     ) ON COMMIT DROP;

    --     FOREACH _word IN ARRAY _words LOOP
    --         INSERT INTO t1
    --             SELECT DISTINCT
    --                 post_id,
    --                 1 score
    --             FROM post_word_index
    --             WHERE word = _word;
    --     END LOOP;

    --     RETURN QUERY
    --         SELECT
    --             post_id,
    --             CAST(SUM(score) AS FLOAT) "rank"
    --         FROM t1
    --         GROUP BY
    --             post_id,
    --             score
    --         ORDER BY
    --             "rank" DESC;

    --     RAISE NOTICE 'test';
    -- END
    -- $$
    -- LANGUAGE 'plpgsql';

    -- APPROACH 1: best_match
    SELECT post_id, SUM(score) rank FROM (
        (SELECT DISTINCT ON (post_id) post_id, 1 score FROM post_word_index WHERE word = 'unit')
        UNION ALL
        (SELECT DISTINCT ON (post_id)  post_id, 1 score FROM post_word_index WHERE word = 'test')
        UNION ALL
        (SELECT DISTINCT ON (post_id)  post_id, 1 score FROM post_word_index WHERE word = 'sql')
    ) t1
    GROUP BY post_id, score
    ORDER BY rank DESC;

    -- APPROACH 2: best_match
    -- Don't know how to do a query with it, but essentially you just
    -- aggregate all the post_ids+words in a temporary table instead
    -- of doing a union between multiple tables.



--
-- ranked_weighted_match
--
CREATE OR REPLACE FUNCTION ranked_weighted_match(_words TEXT[])
RETURNS SETOF match_result AS
$$
BEGIN
    RETURN QUERY
        SELECT
            post_id,
            SUM(tf_idf) "rank"
        FROM get_ranked_weighted_match_index(_words)
        GROUP BY post_id
        ORDER BY "rank" DESC;
END
$$
LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION get_ranked_weighted_match_index(_words TEXT[])
RETURNS TABLE (post_id INT, tf_idf FLOAT) AS
$$
DECLARE
    _word TEXT;
BEGIN
    FOREACH _word IN ARRAY _words LOOP
        RETURN QUERY
            SELECT DISTINCT
                pwi.post_id,
                pwi.tf_idf
            FROM post_word_index pwi
            WHERE word = _word;
    END LOOP;
END
$$
LANGUAGE 'plpgsql';