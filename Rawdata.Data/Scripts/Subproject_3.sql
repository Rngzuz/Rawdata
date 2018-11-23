--
-- post_words_cleaned
--
select * into post_words_cleaned from words
where tablename = 'posts'
    and lower(word) not in (select * from stopwords)
    and (
        lower(word) in (select * from tags)
        or ((length(word) > 2 and word ~*'^[a-zA-Z]+$'))
    );

alter table post_words_cleaned add foreign key (id) references posts (id);
create index post_words_cleaned_word_index on post_words_cleaned (word);
create index post_words_cleaned_post_id_index on post_words_cleaned (id);

--
-- comment_words_cleaned
--
select * into comment_words_cleaned from words
where tablename = 'comments'
    and lower(word) not in (select * from stopwords)
    and (
        lower(word) in (select * from tags)
        or ((length(word) > 2 and word ~*'^[a-zA-Z]+$'))
    );

alter table comment_words_cleaned add foreign key (id) references comments (id);
create index comment_words_cleaned_word_index on comment_words_cleaned (word);
create index comment_words_cleaned_comment_id_index on comment_words_cleaned (id);

--
-- post_word_count
--
create table post_word_count as
select
	word,
	count(word) count
from (select lower(word) word from post_words_cleaned) words
group by word
order by count desc;

alter table post_word_count
    add primary key (word);


--
-- B3
--
create or replace function calculate_post_tf(query varchar, postId int)
returns float as $$
declare
    _post_frequency int := 0;
    _post_count int := 0;
    _total_count int;

    _collection post_words_cleaned;
begin
    -- get body and title frequenies and set to 0 if they are null
    select into _post_frequency count(id) from post_words_cleaned where id = postId and lower(word) = lower(query) group by id;

    if _post_frequency IS NULL then
        _post_frequency := 0;
    end if;

    -- get body and title counts and set to 0 if they are null
    select into _post_count count(id) from post_words_cleaned where id = postId group by id;

    if _post_count IS NULL then
        _post_count := 0;
    end if;

    -- tf(d, t) = log(1 + (n(d, t)/n(d)))
    return LOG(1 + (cast(_post_frequency as float) / _post_count));
end
$$ language 'plpgsql';


create or replace function calculate_post_idf(query varchar)
returns float as $$
declare
    _post_count int;
begin
    select into _post_count count from post_word_count where lower(word) = lower(query);
    return 1 / (cast(_post_count as float));
end
$$ language 'plpgsql';

--
-- post_word_index
--
create table post_word_index as
select
    id post_id,
    what context,
    lower(word) word,
    sen sentence,
    idx "index",
    (calculate_post_tf(word, id) * calculate_post_idf(word)) tf_idf
from post_words_cleaned;

create index pwi_post_id_index on post_word_index (post_id);
create index pwi_word_index on post_word_index (word);

alter table post_word_index add foreign key (word) references post_word_count (word);


--
-- B1
--
create or replace function exact_match(_words text[])
returns table (post_id int) as $$
declare
   _word text;
   _sub_queries text[];
   _query text := 'select posts.id from posts, ';
   _count int := 0;
begin
   foreach _word in array _words loop
       _sub_queries := array_append(
           _sub_queries,
           '(select post_id from post_word_index where word = ''' || _word || ''') t' || _count
       );

       _count := _count + 1;
   end loop;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' join ') || ' using (post_id))'
       || ' t_all where t_all.post_id = posts.id group by posts.id order by score desc';

   raise notice '%', _query;
   return query execute _query;
end
$$ language 'plpgsql';

--
-- B2
--
create or replace function best_match(_words text[])
returns table (post_id int, "rank" float) as $$
declare
   _word text;
   _sub_queries text[];
   _query text := 'select posts.id, cast(sum(t_all.score) as float) rank from posts, ';
begin
   foreach _word in array _words loop
       _sub_queries := array_append(
           _sub_queries,
           '(select distinct post_id, 1 score from post_word_index where word = ''' || _word || ''')'
       );
   end loop;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' union all ') || ')'
       || ' t_all where t_all.post_id = posts.id group by posts.id order by rank desc, posts.score desc';

   raise notice '%', _query;
   return query execute _query;
end
$$ language 'plpgsql';

--
-- B4
--
create or replace function ranked_weighted_match(_words text[])
returns table (post_id int, "rank" float) as $$
declare
   _word text;
   _sub_queries text[];
   _query text := 'select posts.id, sum(t_all.tf_idf) rank from posts, ';
begin
   foreach _word in array _words loop
       _sub_queries := array_append(
           _sub_queries,
           '(select distinct post_id, tf_idf from post_word_index where word = ''' || _word || ''')'
       );
   end loop;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' union all ') || ')'
       || ' t_all where t_all.post_id = posts.id group by posts.id order by rank desc, posts.score desc';

   raise notice '%', _query;
   return query execute _query;
end
$$ language 'plpgsql';


--
-- B5
--
create or replace function word_to_word(_word text)
returns table(word text, freq bigint) as $$
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
$$ language 'plpgsql';

--
--B7
--
create table post_word_association as
    select w1.word word1, w2.word word2, count(*) as grade
    from post_word_index w1, post_word_index w2
    left join post_word_count pf
        using (word)
    where w1.post_id = w2.post_id and w1.word < w2.word
    and w1.tf_idf > 0.00002 and w2.tf_idf > 0.00002 and pf.count > 20
    group by w1.word, w2.word order by grade desc;

create index pwa_word1_index on post_word_association (word1);
create index pwa_word2_index on post_word_association (word2);

create or replace function get_word_association(_word text)
returns setof post_word_association as $$
begin
    return query
        select * from post_word_association where word1 = lower(_word)
        union
        select * from post_word_association where word2 = lower(_word)
        order by grade desc;
end
$$ language 'plpgsql';
