create or replace function exact_match(variadic _words text[])
returns table (id int, title text, body text, score int) as $$
declare
   _word text;
   _sub_queries text[];
   _query text := 'select posts.id, title, body, score from posts, ';
   _count int := 0;
begin
   foreach _word in array _words loop
       _sub_queries := array_append(
           _sub_queries,
           '(select id from mwi where word = ''' || _word || ''' union all select id from mwib where word = ''' || _word || ''') t' || _count
       );

       _count := _count + 1;
   end loop;

   _query := _query
       || ' (' || array_to_string(_sub_queries, ' join ') || ' using (id))'
       || ' t_all where t_all.id=posts.id group by posts.id order by score desc';

   raise notice '%', _query;
   return query execute _query;
end
$$ language 'plpgsql';
