-- Setup BizStack databases on existing PostgreSQL
-- Run with: sudo -u postgres psql -f setup-existing-postgres.sql

-- Create missing databases (PostgreSQL doesn't support IF NOT EXISTS for databases)
-- Check and create databases
SELECT 'CREATE DATABASE auth_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'auth_db')\gexec
SELECT 'CREATE DATABASE user_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'user_db')\gexec
SELECT 'CREATE DATABASE organization_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'organization_db')\gexec
SELECT 'CREATE DATABASE product_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'product_db')\gexec
SELECT 'CREATE DATABASE customer_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'customer_db')\gexec
SELECT 'CREATE DATABASE transaction_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'transaction_db')\gexec
SELECT 'CREATE DATABASE notification_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'notification_db')\gexec
SELECT 'CREATE DATABASE filestorage_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'filestorage_db')\gexec
SELECT 'CREATE DATABASE n8n_db' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'n8n_db')\gexec

-- Create bizstack user if not exists
DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'bizstack_admin') THEN
        CREATE USER bizstack_admin WITH PASSWORD 'bizstack_pass123';
    END IF;
END
$$;

-- Grant permissions
GRANT ALL PRIVILEGES ON DATABASE auth_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE user_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE organization_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE product_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE customer_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE transaction_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE notification_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE filestorage_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE n8n_db TO bizstack_admin;

-- Create extensions for each database
\c auth_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c user_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c organization_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c product_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c customer_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c transaction_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c notification_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c filestorage_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
