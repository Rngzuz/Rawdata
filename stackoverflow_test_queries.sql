-- insert dummy users
insert into users
    (display_name, creation_date, email, "password", deactivation_date)
values
    ('Begoña', now()::timestamp(0), 'begona@test.local', '123', null),
    ('Tobias', now()::timestamp(0), 'tobias@test.local', '123', null),
    ('Tomas', now()::timestamp(0), 'tomas@test.local', '123', null);

-- add_to_search_history
select * from searches where user_id = 2;
select add_to_search_history(2, 'asp.net mvc');
select * from searches where user_id = 2;

-- filter_by_words
select * from searches where user_id = 1;
select * from filter_by_words(1, 'testing unit', array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]);
select * from searches where user_id = 1;

-- filter_by_tags
select * from filter_by_tags(array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]);

-- favorite_comment
select * from favorite_comments where user_id = 2 and comment_id = 31044642;
select favorite_comment(2, 31044642, 'Ineteresting revelation!');
select * from favorite_comments where user_id = 2 and comment_id = 31044642;

-- favorite_post
select * from favorite_posts where user_id = 2 and post_id = 231855;
select favorite_post(2, 231855, 'This is the post with the highest score.');
select * from favorite_posts where user_id = 2 and post_id = 231855;

-- register_user
select * from get_user_by_email('sophie@test.local');
select register_user('Sophie', 'sophie@test.local', '123');
select * from get_user_by_email('sophie@test.local');

-- get_user_by_email
select * from get_user_by_email('tomas@test.local');

-- deactivate_user
select * from users where id = 2;
select * from searches where user_id = 2;
select * from favorite_comments where user_id = 2;
select * from favorite_posts where user_id = 2;
select deactivate_user(2);
select * from deactivated_users where id = 2;
select * from deactivated_searches where user_id = 2;
select * from deactivated_favorite_comments where user_id = 2;
select * from deactivated_favorite_posts where user_id = 2;

-- reactivate_user
select * from deactivated_users where id = 2;
select * from deactivated_searches where user_id = 2;
select * from deactivated_favorite_comments where user_id = 2;
select * from deactivated_favorite_posts where user_id = 2;
select reactivate_user(2);
select * from users where id = 2;
select * from searches where user_id = 2;
select * from favorite_comments where user_id = 2;
select * from favorite_posts where user_id = 2;