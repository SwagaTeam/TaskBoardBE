-- init/init-fdw.sql

-- Выполняется внутри каждой базы, используем функцию DO для динамического подключения

\connect projects_db;

CREATE EXTENSION IF NOT EXISTS postgres_fdw;

-- Подключаем users_db
DROP SERVER IF EXISTS users_server CASCADE;
CREATE SERVER users_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'users_db', port '5432');
CREATE USER MAPPING FOR postgres SERVER users_server OPTIONS (user 'postgres', password 'postgres');

IMPORT FOREIGN SCHEMA public
FROM SERVER users_server INTO public;

-- Подключаем analytics_db
DROP SERVER IF EXISTS analytics_server CASCADE;
CREATE SERVER analytics_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'analytics_db', port '5432');
CREATE USER MAPPING FOR postgres SERVER analytics_server OPTIONS (user 'postgres', password 'postgres');

IMPORT FOREIGN SCHEMA public
FROM SERVER analytics_server INTO public;

--------------------------------------------------------------------------------

\connect analytics_db;

CREATE EXTENSION IF NOT EXISTS postgres_fdw;

-- Подключаем users_db
DROP SERVER IF EXISTS users_server CASCADE;
CREATE SERVER users_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'users_db', port '5432');
CREATE USER MAPPING FOR postgres SERVER users_server OPTIONS (user 'postgres', password 'postgres');

IMPORT FOREIGN SCHEMA public
FROM SERVER users_server INTO public;

-- Подключаем projects_db
DROP SERVER IF EXISTS projects_server CASCADE;
CREATE SERVER projects_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'projects_db', port '5432');
CREATE USER MAPPING FOR postgres SERVER projects_server OPTIONS (user 'postgres', password 'postgres');

IMPORT FOREIGN SCHEMA public
FROM SERVER projects_server INTO public;

--------------------------------------------------------------------------------

\connect users_db;

CREATE EXTENSION IF NOT EXISTS postgres_fdw;

-- Подключаем projects_db
DROP SERVER IF EXISTS projects_server CASCADE;
CREATE SERVER projects_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'projects_db', port '5432');
CREATE USER MAPPING FOR postgres SERVER projects_server OPTIONS (user 'postgres', password 'postgres');

IMPORT FOREIGN SCHEMA public
FROM SERVER projects_server INTO public;

-- Подключаем analytics_db
DROP SERVER IF EXISTS analytics_server CASCADE;
CREATE SERVER analytics_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'analytics_db', port '5432');
CREATE USER MAPPING FOR postgres SERVER analytics_server OPTIONS (user 'postgres', password 'postgres');

IMPORT FOREIGN SCHEMA public
FROM SERVER analytics_server INTO public;
