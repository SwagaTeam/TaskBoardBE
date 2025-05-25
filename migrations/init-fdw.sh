#!/usr/bin/env bash
set -euo pipefail

# Подставьте ваш пароль здесь (или передавайте из docker-compose через environment)
export PGPASSWORD='postgres'

# Хосты и соответствующие им имена баз
HOSTS=(project-db user-db analytics-db)
DBS=(projects_db users_db analytics_db)

# Проверяем доступность каждого порта
for host in "${HOSTS[@]}"; do
  echo "Ожидаем $host:5432..."
  until pg_isready -h "$host" -U postgres; do
    sleep 1
  done
done

# Выполняем FDW-инициализацию в каждой БД
for idx in "${!DBS[@]}"; do
  HOST="${HOSTS[$idx]}"
  DB="${DBS[$idx]}"
  echo "=== Настройка FDW в базе '$DB' на хосте '$HOST' ==="

  psql -h "$HOST" -U postgres -d "$DB" <<-EOSQL
    CREATE EXTENSION IF NOT EXISTS postgres_fdw;

    -- users_server
    DROP SERVER IF EXISTS users_server CASCADE;
    CREATE SERVER users_server FOREIGN DATA WRAPPER postgres_fdw
      OPTIONS (host 'user-db', dbname 'users_db', port '5432');
    CREATE USER MAPPING FOR CURRENT_USER SERVER users_server
      OPTIONS (user 'postgres', password 'postgres');

    -- projects_server
    DROP SERVER IF EXISTS projects_server CASCADE;
    CREATE SERVER projects_server FOREIGN DATA WRAPPER postgres_fdw
      OPTIONS (host 'project-db', dbname 'projects_db', port '5432');
    CREATE USER MAPPING FOR CURRENT_USER SERVER projects_server
      OPTIONS (user 'postgres', password 'postgres');

    -- analytics_server
    DROP SERVER IF EXISTS analytics_server CASCADE;
    CREATE SERVER analytics_server FOREIGN DATA WRAPPER postgres_fdw
      OPTIONS (host 'analytics-db', dbname 'analytics_db', port '5432');
    CREATE USER MAPPING FOR CURRENT_USER SERVER analytics_server
      OPTIONS (user 'postgres', password 'postgres');

    -- импорт всех внешних схем
    IMPORT FOREIGN SCHEMA public FROM SERVER users_server INTO public;
    IMPORT FOREIGN SCHEMA public FROM SERVER projects_server INTO public;
    IMPORT FOREIGN SCHEMA public FROM SERVER analytics_server INTO public;
EOSQL

done

echo ">>> FDW инициализация завершена."