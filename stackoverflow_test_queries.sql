-- Filter test queries
select * from filter_by_words(
    'testing unit',
    array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]
);

select * from filter_by_words('testing unit');

select * from filter_by_tags(
    array['.net', '.htaccess', 'ajax', 'javascript', 'c#', 'sql']::text[]
);
