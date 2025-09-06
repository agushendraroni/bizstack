#!/bin/bash

# ==============================
# BizStack MVP Framework Setup
# ==============================

YELLOW='\033[1;33m'
GREEN='\033[1;32m'
RED='\033[1;31m'
NC='\033[0m'

echo -e "${YELLOW}🚀 Setting up BizStack MVP Framework...${NC}"

# Test GraphQL Mesh
echo -e "${YELLOW}🔗 Testing GraphQL Mesh integration...${NC}"
if curl -s http://localhost:4000/ >/dev/null 2>&1; then
    echo -e "${GREEN}✅ GraphQL Mesh is running${NC}"
    echo -e "${YELLOW}📋 GraphQL Playground: http://localhost:4000${NC}"
else
    echo -e "${RED}❌ GraphQL Mesh is not accessible${NC}"
fi

# Test n8n
echo -e "${YELLOW}🔄 Testing n8n workflow engine...${NC}"
if curl -s http://localhost:5678/ >/dev/null 2>&1; then
    echo -e "${GREEN}✅ n8n is running${NC}"
    echo -e "${YELLOW}📋 n8n Interface: http://localhost:5678${NC}"
    echo -e "${YELLOW}   Username: admin${NC}"
    echo -e "${YELLOW}   Password: admin123${NC}"
else
    echo -e "${RED}❌ n8n is not accessible${NC}"
fi

# Test all backend services
echo -e "${YELLOW}🔍 Verifying all backend services...${NC}"
SERVICES=(
    "auth-service:5001"
    "user-service:5002"
    "organization-service:5003"
    "product-service:5004"
    "customer-service:5005"
    "transaction-service:5006"
    "report-service:5007"
    "notification-service:5008"
    "file-storage-service:5009"
)

ALL_WORKING=true
for service_port in "${SERVICES[@]}"; do
    IFS=':' read -r service port <<< "$service_port"
    if curl -s "http://localhost:$port/health" >/dev/null 2>&1; then
        echo -e "${GREEN}✅ $service${NC}"
    else
        echo -e "${RED}❌ $service${NC}"
        ALL_WORKING=false
    fi
done

if [ "$ALL_WORKING" = true ]; then
    echo -e "${GREEN}🎉 All services are running perfectly!${NC}"
else
    echo -e "${RED}⚠️  Some services need attention${NC}"
fi

echo -e "\n${YELLOW}📋 BizStack MVP Framework URLs:${NC}"
echo -e "• GraphQL API Gateway: http://localhost:4000"
echo -e "• n8n Workflow Engine: http://localhost:5678"
echo -e "• Auth Service: http://localhost:5001"
echo -e "• User Service: http://localhost:5002"
echo -e "• Organization Service: http://localhost:5003"
echo -e "• Product Service: http://localhost:5004"
echo -e "• Customer Service: http://localhost:5005"
echo -e "• Transaction Service: http://localhost:5006"
echo -e "• Report Service: http://localhost:5007"
echo -e "• Notification Service: http://localhost:5008"
echo -e "• File Storage Service: http://localhost:5009"

echo -e "\n${GREEN}🎯 MVP Framework Ready for:${NC}"
echo -e "• Retail/UMKM businesses"
echo -e "• Restaurant/F&B operations"
echo -e "• Service-based businesses"
echo -e "• Educational institutions"
echo -e "• Any small-medium business model"

echo -e "\n${YELLOW}📖 Next Steps:${NC}"
echo -e "1. Access GraphQL Playground at http://localhost:4000"
echo -e "2. Setup n8n workflows at http://localhost:5678"
echo -e "3. Configure business-specific automations"
echo -e "4. Deploy to production environment"