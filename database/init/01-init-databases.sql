-- BizStack Database Initialization Script
-- Creates separate databases for each microservice

-- Create databases for each service
CREATE DATABASE auth_db;
CREATE DATABASE user_db;
CREATE DATABASE organization_db;
CREATE DATABASE product_db;
CREATE DATABASE customer_db;
CREATE DATABASE transaction_db;
CREATE DATABASE notification_db;
CREATE DATABASE filestorage_db;
CREATE DATABASE n8n_db;

-- Grant permissions to bizstack_admin user
GRANT ALL PRIVILEGES ON DATABASE auth_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE user_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE organization_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE product_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE customer_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE transaction_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE notification_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE filestorage_db TO bizstack_admin;
GRANT ALL PRIVILEGES ON DATABASE n8n_db TO bizstack_admin;

-- Create extensions if needed
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

\c n8n_db;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
