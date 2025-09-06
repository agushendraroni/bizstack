#!/bin/bash

# ==============================
# BizStack Database Setup
# ==============================

YELLOW='\033[1;33m'
GREEN='\033[1;32m'
RED='\033[1;31m'
NC='\033[0m'

echo -e "${YELLOW}🗄️  Setting up BizStack PostgreSQL databases...${NC}"

# Check if PostgreSQL is running
if ! pg_isready -h localhost -p 5432 >/dev/null 2>&1; then
    echo -e "${RED}❌ PostgreSQL is not running on localhost:5432${NC}"
    echo -e "${YELLOW}Please start PostgreSQL first:${NC}"
    echo "  sudo systemctl start postgresql"
    echo "  # or"
    echo "  brew services start postgresql"
    exit 1
fi

echo -e "${GREEN}✅ PostgreSQL is running${NC}"

# Setup databases
echo -e "${YELLOW}📋 Creating databases and user...${NC}"

# Try to run the setup script
if sudo -u postgres psql -f database/setup-simple.sql 2>/dev/null; then
    echo -e "${GREEN}✅ Databases created successfully${NC}"
else
    echo -e "${YELLOW}⚠️  Some databases might already exist, continuing...${NC}"
fi

# Test connections
echo -e "${YELLOW}🔍 Testing database connections...${NC}"

DATABASES=("auth_db" "user_db" "organization_db" "product_db" "customer_db" "transaction_db" "notification_db" "filestorage_db" "n8n_db")

for db in "${DATABASES[@]}"; do
    if PGPASSWORD=bizstack_pass123 psql -h localhost -U bizstack_admin -d "$db" -c "SELECT 1;" >/dev/null 2>&1; then
        echo -e "${GREEN}✅ $db - Connected${NC}"
    else
        echo -e "${RED}❌ $db - Connection failed${NC}"
    fi
done

echo -e "${GREEN}🎉 Database setup completed!${NC}"
echo -e "${YELLOW}Next step: Run ./docker-build.sh${NC}"