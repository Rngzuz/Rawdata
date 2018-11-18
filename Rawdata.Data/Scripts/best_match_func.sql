create or replace function best_match(variadic _words text[])
returns table (id int, rank bigint, title text, body text, score int) as $$
declare
   _word text;
   _sub_queries text[];
   _query text := 'select posts.id, sum(t_all.score) rank, title, body, posts.score from posts, ';
begin
   foreach _word in array _words loop
       _sub_queries := array_append(
           _sub_queries,
           '(select distinct id, 1 score from mwi where word = ''' || _word || ''' union all select distinct id, 1 score from mwib where word = ''' || _word || ''')'
       );
   end loop;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' union all ') || ')'
       || ' t_all where t_all.id=posts.id group by posts.id order by rank desc, score desc';

   raise notice '%', _query;
   return query execute _query;
end
$$ language 'plpgsql';