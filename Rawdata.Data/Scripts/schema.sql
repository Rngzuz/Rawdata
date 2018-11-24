DROP VIEW IF EXISTS posts_with_tags CASCADE;

DROP TABLE IF EXISTS
    authors,
    comments,
    marked_comments,
    marked_posts,
    posts,
    posts_tags,
    searches,
    tags,
    users
    CASCADE;

DROP FUNCTION IF EXISTS
    calculate_post_tf,
    calculate_post_idf
    CASCADE;


--
-- authors
--
SELECT
    t1.ownerid "id",
    t1.ownerdisplayname display_name,
    t1.ownercreationdate creation_date,
    t1.ownerlocation "location",
    t1.ownerage age
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

ALTER TABLE authors ADD PRIMARY KEY ("id");

INSERT INTO authors (
    "id",
    display_name,
    creation_date,
    "location",
    age
)
SELECT
    t2.authorid "id",
    t2.authordisplayname display_name,
    t2.authorcreationdate creation_date,
    t2.authorlocation "location",
    t2.authorage age
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

--
-- posts
--
SELECT DISTINCT ON ("id")
    p."id",
    p.posttypeid "type_id",
    p.parentid parent_id,
    p.acceptedanswerid accepted_answer_id,
    p.creationdate creation_date,
    p.score,
    p.body,
    p.closeddate closed_date,
    p.title,
    p.ownerid author_id,
    p.linkpostid link_id
INTO posts
FROM posts_universal p;

UPDATE posts SET accepted_answer_id = null WHERE accepted_answer_id NOT IN (SELECT "id" FROM posts);
UPDATE posts SET link_id = null WHERE link_id NOT IN (SELECT "id" FROM posts);

ALTER TABLE posts
    ADD PRIMARY KEY ("id"),
    ADD FOREIGN KEY (parent_id) REFERENCES posts ("id") ON DELETE CASCADE,
    ADD FOREIGN KEY (accepted_answer_id) REFERENCES posts ("id") ON DELETE SET NULL,
    ADD FOREIGN KEY (author_id) REFERENCES authors ("id") ON DELETE SET NULL,
    ADD FOREIGN KEY (link_id) REFERENCES posts ("id") ON DELETE SET NULL;

--
-- comments
--
SELECT
    c.commentid "id",
    c.commentscore score,
    c.postid post_id,
    c.commenttext "text",
    c.commentcreatedate creation_date,
    c.authorid author_id
INTO "comments"
FROM comments_universal c;

ALTER TABLE comments
    ADD PRIMARY KEY ("id"),
    ADD FOREIGN KEY (post_id) REFERENCES posts ("id"),
    ADD FOREIGN KEY (author_id) REFERENCES authors ("id");

--
-- tags
--
SELECT DISTINCT
    UNNEST(string_to_array(p.tags, '::')) "name"
INTO tags
FROM posts_universal p
WHERE p.tags IS NOT NULL;

ALTER TABLE tags ADD PRIMARY KEY ("name");

--
-- posts_tags
--
SELECT DISTINCT
    p."id" post_id,
    UNNEST(string_to_array(p.tags, '::')) "name"
INTO posts_tags
FROM posts_universal p
WHERE p.tags IS NOT NULL;

ALTER TABLE posts_tags
    ADD FOREIGN KEY ("name") REFERENCES tags ("name"),
    ADD FOREIGN KEY (post_id) REFERENCES posts ("id");

--
-- users
--
CREATE TABLE users (
    "id" SERIAL PRIMARY KEY,
    display_name TEXT,
    creation_date TIMESTAMP WITHOUT TIME ZONE,
    email TEXT,
    "password" TEXT,

    UNIQUE(email)
);

--
-- marked_posts
--
CREATE TABLE marked_posts (
    "user_id" INTEGER REFERENCES users ("id"),
    post_id INTEGER REFERENCES posts ("id"),
    note TEXT,

    UNIQUE ("user_id", post_id)
);

--
-- marked_comments
--
CREATE TABLE marked_comments (
    "user_id" INTEGER REFERENCES users ("id"),
    comment_id INTEGER REFERENCES comments ("id"),
    note TEXT,

    UNIQUE ("user_id", comment_id)
);

--
-- searches
--
CREATE TABLE searches (
    "user_id" INTEGER REFERENCES users (id),
    search_text TEXT,

    UNIQUE ("user_id", search_text)
);

--
-- stopwords
--
CREATE TABLE stopwords (word varchar(18) DEFAULT NULL);
INSERT INTO
    stopwords
VALUES
    ('a'),('a''s'),('able'),('about'),('above'),('according'),('accordingly'),('across'),('actually'),('after'),('afterwards'),('again'),('against'),('ain''t'),('all'),('allow'),('allows'),('almost'),('alone'),('along'),('already'),('also'),('although'),('always'),
    ('am'),('among'),('amongst'),('an'),('and'),('another'),('any'),('anybody'),('anyhow'),('anyone'),('anything'),('anyway'),('anyways'),('anywhere'),('apart'),('appear'),('appreciate'),('appropriate'),('are'),('aren''t'),('around'),('as'),('aside'),('ask'),('asking'),('associated'),('at'),('available'),('away'),('awfully'),('be'),('became'),('because'),('become'),('becomes'),('becoming'),('been'),('before'),('beforehand'),('behind'),('being'),('believe'),('below'),('beside'),('besides'),('best'),('better'),('between'),('beyond'),('both'),('brief'),('but'),('by'),('c''mon'),('c''s'),('came'),('can'),('can''t'),('cannot'),('cant'),('cause'),('causes'),('certain'),('certainly'),('changes'),('clearly'),('co'),('com'),('come'),('comes'),('concerning'),('consequently'),('consider'),('considering'),('contain'),('containing'),('contains'),('corresponding'),('could'),('couldn''t'),('course'),('currently'),('definitely'),('described'),('despite'),('did'),('didn''t'),('different'),('do'),('does'),('doesn''t'),('doing'),('don''t'),('done'),('down'),('downwards'),('during'),('each'),('edu'),('eg'),('eight'),('either'),('else'),('elsewhere'),('enough'),('entirely'),('especially'),('et'),('etc'),('even'),('ever'),('every'),('everybody'),('everyone'),('everything'),('everywhere'),('ex'),('exactly'),('example'),('except'),('far'),('few'),('fifth'),('first'),('five'),('followed'),('following'),('follows'),('for'),('former'),('formerly'),('forth'),('four'),('from'),('further'),('furthermore'),('get'),('gets'),('getting'),('given'),('gives'),('go'),('goes'),('going'),('gone'),('got'),('gotten'),('greetings'),('had'),('hadn''t'),('happens'),('hardly'),('has'),('hasn''t'),('have'),('haven''t'),('having'),('he'),('he''s'),('hello'),('help'),('hence'),('her'),('here'),('here''s'),('hereafter'),('hereby'),('herein'),('hereupon'),('hers'),('herself'),('hi'),('him'),('himself'),('his'),('hither'),('hopefully'),('how'),('howbeit'),('however'),('i''d'),('i''ll'),('i''m'),('i''ve'),('ie'),('if'),('ignored'),('immediate'),('in'),('inasmuch'),('inc'),('indeed'),('indicate'),('indicated'),('indicates'),('inner'),('insofar'),('instead'),('into'),('inward'),('is'),('isn''t'),('it'),('it''d'),('it''ll'),('it''s'),('its'),('itself'),('just'),('keep'),('keeps'),('kept'),('know'),('knows'),('known'),('last'),('lately'),('later'),('latter'),('latterly'),('least'),('less'),('lest'),('let'),('let''s'),('like'),('liked'),('likely'),('little'),('look'),('looking'),('looks'),('ltd'),('mainly'),('many'),('may'),('maybe'),('me'),('mean'),('meanwhile'),('merely'),('might'),('more'),('moreover'),('most'),('mostly'),('much'),('must'),('my'),('myself'),('name'),('namely'),('nd'),('near'),('nearly'),('necessary'),('need'),('needs'),('neither'),('never'),('nevertheless'),('new'),('next'),('nine'),('no'),('nobody'),('non'),('none'),('noone'),('nor'),('normally'),('not'),('nothing'),('novel'),('now'),('nowhere'),('obviously'),('of'),('off'),('often'),('oh'),('ok'),('okay'),('old'),('on'),('once'),('one'),('ones'),('only'),('onto'),('or'),('other'),('others'),('otherwise'),('ought'),('our'),('ours'),('ourselves'),('out'),('outside'),('over'),('overall'),('own'),('particular'),('particularly'),('per'),('perhaps'),('placed'),('please'),('plus'),('possible'),('presumably'),('probably'),('provides'),('que'),('quite'),('qv'),('rather'),('rd'),('re'),('really'),('reasonably'),('regarding'),('regardless'),('regards'),('relatively'),('respectively'),('right'),('said'),('same'),('saw'),('say'),('saying'),('says'),('second'),('secondly'),('see'),('seeing'),('seem'),('seemed'),('seeming'),('seems'),('seen'),('self'),('selves'),('sensible'),('sent'),('serious'),('seriously'),('seven'),('several'),('shall'),('she'),('should'),('shouldn''t'),('since'),('six'),('so'),('some'),('somebody'),('somehow'),('someone'),('something'),('sometime'),('sometimes'),('somewhat'),('somewhere'),('soon'),('sorry'),('specified'),('specify'),('specifying'),('still'),('sub'),('such'),('sup'),('sure'),('t''s'),('take'),('taken'),('tell'),('tends'),('th'),('than'),('thank'),('thanks'),('thanx'),('that'),('that''s'),('thats'),('the'),('their'),('theirs'),('them'),('themselves'),('then'),('thence'),('there'),('there''s'),('thereafter'),('thereby'),('therefore'),('therein'),('theres'),('thereupon'),('these'),('they'),('they''d'),('they''ll'),('they''re'),('they''ve'),('think'),('third'),('this'),('thorough'),('thoroughly'),('those'),('though'),('three'),('through'),('throughout'),('thru'),('thus'),('to'),('together'),('too'),('took'),('toward'),('towards'),('tried'),('tries'),('truly'),('try'),('trying'),('twice'),('two'),('un'),('under'),('unfortunately'),('unless'),('unlikely'),('until'),('unto'),('up'),('upon'),('us'),('use'),('used'),('useful'),('uses'),('using'),('usually'),('value'),('various'),('very'),('via'),('viz'),('vs'),('want'),('wants'),('was'),('wasn''t'),('way'),('we'),('we''d'),('we''ll'),('we''re'),('we''ve'),('welcome'),('well'),('went'),('were'),('weren''t'),('what'),('what''s'),('whatever'),('when'),('whence'),('whenever'),('where'),('where''s'),('whereafter'),('whereas'),('whereby'),('wherein'),('whereupon'),('wherever'),('whether'),('which'),('while'),('whither'),('who'),('who''s'),('whoever'),('whole'),('whom'),('whose'),('why'),('will'),('willing'),('wish'),('with'),('within'),('without'),('won''t'),('wonder'),('would'),('wouldn''t'),('yes'),('yet'),('you'),('you''d'),('you''ll'),('you''re'),('you''ve'),('your'),('yours'),('yourself'),('yourselves'),('zero');

--
-- post_words_cleaned
--
SELECT * INTO post_words_cleaned FROM words
WHERE tablename = 'posts'
    AND LOWER(word) NOT IN (SELECT * FROM stopwords)
    AND (
        LOWER(word) IN (SELECT * FROM tags)
        OR ((LENGTH(word) > 2 AND word ~* '^[a-zA-Z]+$'))
    );

ALTER TABLE post_words_cleaned ADD FOREIGN KEY (id) REFERENCES posts (id);
CREATE INDEX post_words_cleaned_word_index ON post_words_cleaned (word);
CREATE INDEX post_words_cleaned_post_id_index ON post_words_cleaned (id);

--
-- comment_words_cleaned
--
SELECT * INTO comment_words_cleaned FROM words
WHERE tablename = 'comments'
    AND LOWER(word) NOT IN (SELECT * FROM stopwords)
    AND (
        LOWER(word) iN (SELECT * FROM tags)
        OR ((LENGTH(word) > 2 AND word ~*'^[a-zA-Z]+$'))
    );

ALTER TABLE comment_words_cleaned ADD FOREIGN KEY (id) REFERENCES comments (id);
CREATE INDEX comment_words_cleaned_word_index ON comment_words_cleaned (word);
CREATE INDEX comment_words_cleaned_comment_id_index ON comment_words_cleaned (id);

--
-- post_word_count
--
CREATE TABLE post_word_count AS
SELECT
	word,
	COUNT(word) count
FROM (SELECT LOWER(word) word FROM post_words_cleaned) words
GROUP BY word
ORDER BY count DESC;

ALTER TABLE post_word_count
    ADD PRIMARY KEY (word);

--
-- calculate_post_tf
--
CREATE OR REPLACE FUNCTION calculate_post_tf(query VARCHAR, postId INT)
RETURNS FLOAT AS $$
DECLARE
    _post_frequency INT := 0;
    _post_count INT := 0;
    _total_count INT;

    _collection post_words_cleaned;
BEGIN
    -- get body and title frequenies and set to 0 if they are null
    SELECT INTO _post_frequency COUNT(id) FROM post_words_cleaned WHERE id = postId AND LOWER(word) = LOWER(query) GROUP BY id;

    IF _post_frequency IS NULL THEN
        _post_frequency := 0;
    END IF;

    -- get body and title counts and set to 0 if they are null
    SELECT INTO _post_count count(id) FROM post_words_cleaned WHERE id = postId GROUP BY id;

    IF _post_count IS NULL THEN
        _post_count := 0;
    END IF;

    -- tf(d, t) = log(1 + (n(d, t)/n(d)))
    RETURN LOG(1 + (CAST(_post_frequency AS FLOAT) / _post_count));
END
$$ LANGUAGE 'plpgsql';

--
-- calculate_post_idf
--
CREATE OR REPLACE FUNCTION calculate_post_idf(query VARCHAR)
RETURNS FLOAT AS $$
DECLARE
    _post_count INT;
BEGIN
    SELECT INTO _post_count count FROM post_word_count WHERE LOWER(word) = LOWER(query);
    RETURN 1 / (CAST(_post_count AS FLOAT));
END
$$ LANGUAGE 'plpgsql';

--
-- post_word_index
--
CREATE TABLE post_word_index AS
SELECT
    id post_id,
    what context,
    LOWER(word) word,
    sen sentence,
    idx "index",
    (calculate_post_tf(word, id) * calculate_post_idf(word)) tf_idf
FROM post_words_cleaned;

CREATE INDEX pwi_post_id_index ON post_word_index (post_id);
CREATE INDEX pwi_word_index ON post_word_index (word);

ALTER TABLE post_word_index add FOREIGN KEY (word) REFERENCES post_word_count (word);