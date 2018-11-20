
create or replace function word_to_word(variadic _words text[])
returns table(word text, freq bigint) as $$
BEGIN
RETURN query
SELECT
  t1.word,
  COUNT(*) AS freq
FROM
  tf_idf t1
LEFT JOIN tf_idf t2 ON t1.id = t2.id
where  t2.word = ANY(_words)
GROUP BY t1.word order by freq desc;
END
$$ language 'plpgsql';