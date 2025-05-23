#!/bin/sh
set -e

psql -U postgres -d postgres -c "CREATE DATABASE users_db;"
psql -U postgres -d postgres -c "CREATE DATABASE projects_db;"
psql -U postgres -d postgres -c "CREATE DATABASE analytics_db;"