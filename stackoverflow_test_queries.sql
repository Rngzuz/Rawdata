-- Insert some test users
insert into users
    (display_name, creation_date, email, "password", deactivation_date)
values
    ('Begoña', now()::timestamp(0), 'begona@test.local', '123', null),
    ('Tobias', now()::timestamp(0), 'tobias@test.local', '123', null),
    ('Tomas', now()::timestamp(0), 'tomas@test.local', '123', null);

-- Filter test queries
select * from filter_by_words(
    null /*user_id*/,
    'testing unit',
    array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]
);

select * from filter_by_words(null /*user_id*/, 'testing unit');

select * from filter_by_tags(
    array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]
);

select * from get_posts_with_links1(19);

insert into searches (user_id, search_text) values (1,'Null pointer');
insert into searches (user_id, search_text) values (2,'exception');

select * from get_users_search_history(1);