create or replace function calculate_tf(query varchar, postId int)
returns float as $$
declare
    _body_frequency int := 0;
    _title_frequency int := 0;
    _total_frequency int;
    
    _body_count int := 0;
    _title_count int := 0;
    _total_count int;
begin
    -- get body and title frequenies and set to 0 if they are null
    select into _body_frequency count(*) from mwib where word = query and id = postId group by id;
    select into _title_frequency count(*) from mwi where word = query and id = postId group by id;
    if _body_frequency IS NULL then
        _body_frequency := 0;
    elsif _title_frequency IS NULL then
        _title_frequency := 0;
    end if;
    
    _total_frequency := _body_frequency + _title_frequency;

    -- get body and title counts and set to 0 if they are null
    select into _body_count count(*) from mwib where id = postId group by id;
    select into _title_count count(*) from mwi where id = postId group by id;
    if _body_count IS NULL then
        _body_count := 0;
    elsif _title_count IS NULL then
        _title_count := 0;
    end if;
    
    _total_count := _body_count + _title_count;

    -- tf(d, t) = log(1 + (n(d, t)/n(d)))
    return LOG(1 + (cast(_total_frequency as float) / _total_count));
end
$$ language 'plpgsql';


create or replace function calculate_idf(query varchar)
returns float as $$
declare
    _post_body_count int;
    _post_title_count int;
begin
    select into _post_body_count count(*) from mwib where word = query ;
    select into _post_title_count count(*) from mwi where word = query ;

    return 1 / ((cast(_post_body_count as float) + _post_title_count));
end
$$ language 'plpgsql';


CREATE TABLE stopwords (
  word varchar(18) DEFAULT NULL
);
INSERT INTO stopwords VALUES ('a'),('a''s'),('able'),('about'),('above'),('according'),('accordingly'),('across'),('actually'),('after'),('afterwards'),('again'),('against'),('ain''t'),('all'),('allow'),('allows'),('almost'),('alone'),('along'),('already'),('also'),('although'),('always'),
('am'),('among'),('amongst'),('an'),('and'),('another'),('any'),('anybody'),('anyhow'),('anyone'),('anything'),('anyway'),('anyways'),('anywhere'),('apart'),('appear'),('appreciate'),(
'appropriate'),('are'),('aren''t'),('around'),('as'),('aside'),('ask'),('asking'),('associated'),('at'),('available'),('away'),('awfully'),('be'),('became'),('because'),('become'),('becomes'),('becoming'),('been'),('before'),('beforehand'),('behind'),('being'),('believe'),('below'),('beside'),('besides'),('best'),('better'),('between'),('beyond'),('both'),('brief'),('but'),('by'),('c''mon'),('c''s'),('came'),('can'),('can''t'),('cannot'),('cant'),('cause'),('causes'),('certain'),('certainly'),('changes'),('clearly'),('co'),('com'),('come'),('comes'),('concerning'),('consequently'),('consider'),('considering'),('contain'),('containing'),('contains'),('corresponding'),('could'),('couldn''t'),('course'),('currently'),('definitely'),('described'),('despite'),('did'),('didn''t'),('different'),('do'),('does'),('doesn''t'),('doing'),('don''t'),('done'),('down'),('downwards'),('during'),('each'),('edu'),('eg'),('eight'),('either'),('else'),('elsewhere'),('enough'),('entirely'),('especially'),('et'),('etc'),('even'),('ever'),('every'),('everybody'),('everyone'),('everything'),('everywhere'),('ex'),('exactly'),('example'),('except'),('far'),('few'),('fifth'),('first'),('five'),('followed'),('following'),('follows'),('for'),('former'),('formerly'),('forth'),('four'),('from'),('further'),('furthermore'),('get'),('gets'),('getting'),('given'),('gives'),('go'),('goes'),('going'),('gone'),('got'),('gotten'),('greetings'),('had'),('hadn''t'),('happens'),('hardly'),('has'),('hasn''t'),('have'),('haven''t'),('having'),('he'),('he''s'),('hello'),('help'),('hence'),('her'),('here'),('here''s'),('hereafter'),('hereby'),('herein'),('hereupon'),('hers'),('herself'),('hi'),('him'),('himself'),('his'),('hither'),('hopefully'),('how'),('howbeit'),('however'),('i''d'),('i''ll'),('i''m'),('i''ve'),('ie'),('if'),('ignored'),('immediate'),('in'),('inasmuch'),('inc'),('indeed'),('indicate'),('indicated'),('indicates'),('inner'),('insofar'),('instead'),('into'),('inward'),('is'),('isn''t'),('it'),('it''d'),('it''ll'),('it''s'),('its'),('itself'),('just'),('keep'),('keeps'),('kept'),('know'),('knows'),('known'),('last'),('lately'),('later'),('latter'),('latterly'),('least'),('less'),('lest'),('let'),('let''s'),('like'),('liked'),('likely'),('little'),('look'),('looking'),('looks'),('ltd'),('mainly'),('many'),('may'),('maybe'),('me'),('mean'),('meanwhile'),('merely'),('might'),('more'),('moreover'),('most'),('mostly'),('much'),('must'),('my'),('myself'),('name'),('namely'),('nd'),('near'),('nearly'),('necessary'),('need'),('needs'),('neither'),('never'),('nevertheless'),('new'),('next'),('nine'),('no'),('nobody'),('non'),('none'),('noone'),('nor'),('normally'),('not'),('nothing'),('novel'),('now'),('nowhere'),('obviously'),('of'),('off'),('often'),('oh'),('ok'),('okay'),('old'),('on'),('once'),('one'),('ones'),('only'),('onto'),('or'),('other'),('others'),('otherwise'),('ought'),('our'),('ours'),('ourselves'),('out'),('outside'),('over'),('overall'),('own'),('particular'),('particularly'),('per'),('perhaps'),('placed'),('please'),('plus'),('possible'),('presumably'),('probably'),('provides'),('que'),('quite'),('qv'),('rather'),('rd'),('re'),('really'),('reasonably'),('regarding'),('regardless'),('regards'),('relatively'),('respectively'),('right'),('said'),('same'),('saw'),('say'),('saying'),('says'),('second'),('secondly'),('see'),('seeing'),('seem'),('seemed'),('seeming'),('seems'),('seen'),('self'),('selves'),('sensible'),('sent'),('serious'),('seriously'),('seven'),('several'),('shall'),('she'),('should'),('shouldn''t'),('since'),('six'),('so'),('some'),('somebody'),('somehow'),('someone'),('something'),('sometime'),('sometimes'),('somewhat'),('somewhere'),('soon'),('sorry'),('specified'),('specify'),('specifying'),('still'),('sub'),('such'),('sup'),('sure'),('t''s'),('take'),('taken'),('tell'),('tends'),('th'),('than'),('thank'),('thanks'),('thanx'),('that'),('that''s'),('thats'),('the'),('their'),('theirs'),('them'),('themselves'),('then'),('thence'),('there'),('there''s'),('thereafter'),('thereby'),('therefore'),('therein'),('theres'),('thereupon'),('these'),('they'),('they''d'),('they''ll'),('they''re'),('they''ve'),('think'),('third'),('this'),('thorough'),('thoroughly'),('those'),('though'),('three'),('through'),('throughout'),('thru'),('thus'),('to'),('together'),('too'),('took'),('toward'),('towards'),('tried'),('tries'),('truly'),('try'),('trying'),('twice'),('two'),('un'),('under'),('unfortunately'),('unless'),('unlikely'),('until'),('unto'),('up'),('upon'),('us'),('use'),('used'),('useful'),('uses'),('using'),('usually'),('value'),('various'),('very'),('via'),('viz'),('vs'),('want'),('wants'),('was'),('wasn''t'),('way'),('we'),('we''d'),('we''ll'),('we''re'),('we''ve'),('welcome'),('well'),('went'),('were'),('weren''t'),('what'),('what''s'),('whatever'),('when'),('whence'),('whenever'),('where'),('where''s'),('whereafter'),('whereas'),('whereby'),('wherein'),('whereupon'),('wherever'),('whether'),('which'),('while'),('whither'),('who'),('who''s'),('whoever'),('whole'),('whom'),('whose'),('why'),('will'),('willing'),('wish'),('with'),('within'),('without'),('won''t'),('wonder'),('would'),('wouldn''t'),('yes'),('yet'),('you'),('you''d'),('you''ll'),('you''re'),('you''ve'),('your'),('yours'),('yourself'),('yourselves'),('zero');

create table tf_idf (
    id int,
    word text,
    tfidf float,
    PRIMARY KEY (id, word),
    CONSTRAINT post_id_fkey FOREIGN KEY (id) REFERENCES posts (id)
);

insert into tf_idf select distinct id, word, (calculate_tf(word, id) * calculate_idf(word)) as tfidf from mwi where word ~*'^[a-zA-Z]+$' and lower(word) not in (select * from stopwords);
insert into tf_idf select distinct id, word, (calculate_tf(word, id) * calculate_idf(word)) as tfidf from mwib where word ~*'^[a-zA-Z]+$' and lower(word) not in (select * from stopwords) ON CONFLICT DO NOTHING;