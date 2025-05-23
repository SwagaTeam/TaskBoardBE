echo "Настраиваем postgres_fdw в projects_db..."
psql -U postgres -d projects_db -c "CREATE EXTENSION IF NOT EXISTS postgres_fdw;"

psql -U postgres -d projects_db -c "DROP SERVER IF EXISTS users_server CASCADE;"
psql -U postgres -d projects_db -c "CREATE SERVER users_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'users_db', port '5432');"
psql -U postgres -d projects_db -c "CREATE USER MAPPING FOR postgres SERVER users_server OPTIONS (user 'postgres', password '$PGPASSWORD');"
psql -U postgres -d projects_db -c "IMPORT FOREIGN SCHEMA public FROM SERVER users_server INTO public;"

psql -U postgres -d projects_db -c "DROP SERVER IF EXISTS analytics_server CASCADE;"
psql -U postgres -d projects_db -c "CREATE SERVER analytics_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'analytics_db', port '5432');"
psql -U postgres -d projects_db -c "CREATE USER MAPPING FOR postgres SERVER analytics_server OPTIONS (user 'postgres', password '$PGPASSWORD');"
psql -U postgres -d projects_db -c "IMPORT FOREIGN SCHEMA public FROM SERVER analytics_server INTO public;"

echo "Настраиваем postgres_fdw в analytics_db..."
psql -U postgres -d analytics_db -c "CREATE EXTENSION IF NOT EXISTS postgres_fdw;"

psql -U postgres -d analytics_db -c "DROP SERVER IF EXISTS users_server CASCADE;"
psql -U postgres -d analytics_db -c "CREATE SERVER users_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'users_db', port '5432');"
psql -U postgres -d analytics_db -c "CREATE USER MAPPING FOR postgres SERVER users_server OPTIONS (user 'postgres', password '$PGPASSWORD');"
psql -U postgres -d analytics_db -c "IMPORT FOREIGN SCHEMA public FROM SERVER users_server INTO public;"

psql -U postgres -d analytics_db -c "DROP SERVER IF EXISTS projects_server CASCADE;"
psql -U postgres -d analytics_db -c "CREATE SERVER projects_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'projects_db', port '5432');"
psql -U postgres -d analytics_db -c "CREATE USER MAPPING FOR postgres SERVER projects_server OPTIONS (user 'postgres', password '$PGPASSWORD');"
psql -U postgres -d analytics_db -c "IMPORT FOREIGN SCHEMA public FROM SERVER projects_server INTO public;"

echo "Настраиваем postgres_fdw в users_db..."
psql -U postgres -d users_db -c "CREATE EXTENSION IF NOT EXISTS postgres_fdw;"

psql -U postgres -d users_db -c "DROP SERVER IF EXISTS projects_server CASCADE;"
psql -U postgres -d users_db -c "CREATE SERVER projects_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'projects_db', port '5432');"
psql -U postgres -d users_db -c "CREATE USER MAPPING FOR postgres SERVER projects_server OPTIONS (user 'postgres', password '$PGPASSWORD');"
psql -U postgres -d users_db -c "IMPORT FOREIGN SCHEMA public FROM SERVER projects_server INTO public;"

psql -U postgres -d users_db -c "DROP SERVER IF EXISTS analytics_server CASCADE;"
psql -U postgres -d users_db -c "CREATE SERVER analytics_server FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'analytics_db', port '5432');"
psql -U postgres -d users_db -c "CREATE USER MAPPING FOR postgres SERVER analytics_server OPTIONS (user 'postgres', password '$PGPASSWORD');"
psql -U postgres -d users_db -c "IMPORT FOREIGN SCHEMA public FROM SERVER analytics_server INTO public;"

echo "FDW настройка завершена."