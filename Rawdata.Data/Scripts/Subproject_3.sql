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
-- post_frequency
--
select
    word,
    count(word) frequency
into post_frequency
from post_words_cleaned
group by word
order by frequency desc;

alter table post_frequency
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
begin
    -- get body and title frequenies and set to 0 if they are null
    select into _post_frequency count(id) from post_words_cleaned where word = query and id = postId group by id;
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
    select into _post_count frequency from post_frequency where word = query;
    return 1 / (cast(_post_count as float));
end
$$ language 'plpgsql';

--
-- post_word_index
--
select
    id post_id,
    what context,
    word,
    sen sentence,
    idx "index",
    (calculate_post_tf(word, id) * calculate_post_idf(word)) tf_idf
into post_word_index
from post_words_cleaned;

alter table post_word_index add foreign key (word) references post_frequency ("word");
create index pwi_post_id_index on post_word_index (post_id);
create index pwi_word_index on post_word_index (word);

--
-- B4
--


--
-- B5
--


--
--B7
--
select w1.word, w2.word, count(*) as grade
from post_word_index w1, post_word_index w2
left join post_frequency pf
    using (word)
where w1.id = w2.id and w1.word < w2.word
and w1.tfidf > 0.0002 and w2.tfidf > 0.0002 and pf.frequency > 20
group by w1.word,w2.word order by grade desc;
