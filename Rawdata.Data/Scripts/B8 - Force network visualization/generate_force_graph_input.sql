drop function if exists generate_force_graph_input;
create or replace function generate_force_graph_input(in w varchar(100), n real) 
returns table (line text) as $$
declare
	l text;
begin
line := '{"nodes":[';
return next;
line := '';
return next;
for l in (select '{"id":"'||lower(word2)||'"},' from post_word_association where word1=w and grade>=n)
loop
	line:=l;
	return next;
end loop;

line := '{"id":"'||w||'"},';
return next;
line :=  '],';
return next;
line :=  '"links":[';
return next;

for l in (select '{"source":"'||lower(word1)||'", "target":"'||lower(word2)||'", "value":'||grade/2||'},'  
from (
select * from post_word_association where word1 = w and grade>=n
union
select * from post_word_association 
where word1 in (select word2 from post_word_association where word1=w and grade>=n)
and word2 in (select word2 from post_word_association where word1=w and grade>=n)) t)
loop
	line:=l;
	return next;
end loop;

line := ']}';
return next;
return;
end;
$$
language 'plpgsql';
select generate_force_graph_input('address', 8);


