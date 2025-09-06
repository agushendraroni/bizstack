-- Setup BizStack databases on existing PostgreSQL
-- Run with: sudo -u postgres psql -f setup-existing-postgres.sql

-- Create missing databases
CREATE DATABASE IF NOT EXISTS organization_db;
CREATE DATABASE IF NOT EXISTS product_db;
CREATE DATABASE IF NOT EXISTS customer_db;
CREATE DATABASE IF NOT EXISTS transaction_db;
CREATE DATABASE IF NOT EXISTS notification_db;
CREATE DATABASE IF NOT EXISTS filestorage_db;

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
