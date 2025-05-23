#!/bin/sh

# Ожидаем, пока PostgreSQL на localhost:5432 начнёт принимать соединения
until pg_isready -h localhost -p 5432 -U postgres > /dev/null 2>&1; do
  echo "Ожидание запуска PostgreSQL..."
  sleep 1
done

echo "PostgreSQL готов к подключению."
