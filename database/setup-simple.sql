-- Simple BizStack Database Setup
-- Run with: sudo -u postgres psql -f setup-simple.sql

-- Create user
CREATE USER bizstack_admin WITH PASSWORD 'bizstack_pass123';

-- Create databases
CREATE DATABASE auth_db OWNER bizstack_admin;
CREATE DATABASE user_db OWNER bizstack_admin;
CREATE DATABASE organization_db OWNER bizstack_admin;
CREATE DATABASE product_db OWNER bizstack_admin;
CREATE DATABASE customer_db OWNER bizstack_admin;
CREATE DATABASE transaction_db OWNER bizstack_admin;
CREATE DATABASE notification_db OWNER bizstack_admin;
CREATE DATABASE filestorage_db OWNER bizstack_admin;
CREATE DATABASE n8n_db OWNER bizstack_admin;

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