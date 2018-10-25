-- post_questions table
select distinct on (p.id)
    p.id,
    p.acceptedanswerid accepted_answer_id,
    p.creationdate creation_date,
    p.score,
    p.body,
    p.closeddate closed_date,
    p.title,
    p.ownerid author_id
into post_questions
from posts_universal p
where p.posttypeid = 1;

alter table post_questions
add primary key (id);

-- post_answers table
select distinct on (p.id)
    p.id,
    p.parentid parent_id,
    p.creationdate creation_date,
    p.score,
    p.body,
    p.ownerid author_id
into post_answers
from posts_universal p
where p.posttypeid = 2;

alter table post_answers
add primary key (id);

-- set accepted_answer_id to null if the post_aswers id dies not exist
update post_questions
set accepted_answer_id = null
where accepted_answer_id not in (
    select pa.id
    from post_questions pq
    join post_answers pa
    on pq.id = pa.parent_id
);

-- post_questions constraints
alter table post_questions
add foreign key (accepted_answer_id)
references post_answers(id);

alter table post_questions
add foreign key (author_id)
references authors(id);

-- post_answers constraints
alter table post_answers
add foreign key (parent_id)
references post_questions(id);

alter table post_answers
add foreign key (author_id)
references authors(id);


-- add title tokens for full textsearch post_questions
alter table post_questions
add column title_tokens TSVECTOR;

update post_questions pq1
set title_tokens = to_tsvector(pq1.title)
from post_questions pq2;