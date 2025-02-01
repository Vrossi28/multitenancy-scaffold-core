#!/bin/bash
set -e

psql -U "$POSTGRES_USER" -d postgres -tc "SELECT 1 FROM pg_database WHERE datname = 'VrossiScaffoldDB'" | grep -q 1 || psql -U "$POSTGRES_USER" -d postgres -c "CREATE DATABASE \"VrossiScaffoldDB\";"
