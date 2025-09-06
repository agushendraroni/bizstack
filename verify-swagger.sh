#!/bin/bash

# ==============================
# BizStack Swagger Endpoints Verification
# ==============================

YELLOW='\033[1;33m'
GREEN='\033[1;32m'
RED='\033[1;31m'
NC='\033[0m'

echo -e "${YELLOW}🔍 Verifying BizStack Swagger Endpoints...${NC}"

# Service endpoints from docker-compose.yml
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
  "graphql-mesh:4000"
  "n8n:5678"
  "frontend:3000"
)

check_endpoint() {
  local service=$1
  local port=$2
  local endpoint=$3
  local description=$4
  
  if curl -s -f "http://localhost:$port$endpoint" >/dev/null 2>&1; then
    echo -e "${GREEN}✅ $service ($port) - $description${NC}"
    return 0
  else
    echo -e "${RED}❌ $service ($port) - $description${NC}"
    return 1
  fi
}

echo -e "\n${YELLOW}📋 Checking Health Endpoints:${NC}"
check_endpoint "auth-service" "5001" "/health" "Health Check"
check_endpoint "user-service" "5002" "/health" "Health Check"
check_endpoint "product-service" "5004" "/health" "Health Check"
check_endpoint "customer-service" "5005" "/health" "Health Check"
check_endpoint "transaction-service" "5006" "/health" "Health Check"
check_endpoint "report-service" "5007" "/health" "Health Check"
check_endpoint "notification-service" "5008" "/health" "Health Check"
check_endpoint "file-storage-service" "5009" "/health" "Health Check"

echo -e "\n${YELLOW}📖 Checking Swagger UI Endpoints:${NC}"
check_endpoint "auth-service" "5001" "/" "Swagger UI"
check_endpoint "user-service" "5002" "/" "Swagger UI"
check_endpoint "organization-service" "5003" "/" "Swagger UI"
check_endpoint "product-service" "5004" "/" "Swagger UI"
check_endpoint "customer-service" "5005" "/" "Swagger UI"
check_endpoint "transaction-service" "5006" "/" "Swagger UI"
check_endpoint "report-service" "5007" "/" "Swagger UI"
check_endpoint "notification-service" "5008" "/" "Swagger UI"
check_endpoint "file-storage-service" "5009" "/" "Swagger UI"

echo -e "\n${YELLOW}📄 Checking Swagger JSON Endpoints:${NC}"
check_endpoint "auth-service" "5001" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "user-service" "5002" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "organization-service" "5003" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "product-service" "5004" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "customer-service" "5005" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "transaction-service" "5006" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "report-service" "5007" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "notification-service" "5008" "/swagger/v1/swagger.json" "Swagger JSON"
check_endpoint "file-storage-service" "5009" "/swagger/v1/swagger.json" "Swagger JSON"

echo -e "\n${YELLOW}🌐 Checking Other Services:${NC}"
check_endpoint "graphql-mesh" "4000" "/" "GraphQL Playground"
check_endpoint "n8n" "5678" "/" "n8n Workflow"
check_endpoint "frontend" "3000" "/" "React Frontend"

echo -e "\n${GREEN}🎉 Swagger verification completed!${NC}"
echo -e "${YELLOW}📋 Service URLs:${NC}"
echo -e "• Auth Service: http://localhost:5001"
echo -e "• User Service: http://localhost:5002"
echo -e "• Organization Service: http://localhost:5003"
echo -e "• Product Service: http://localhost:5004"
echo -e "• Customer Service: http://localhost:5005"
echo -e "• Transaction Service: http://localhost:5006"
echo -e "• Report Service: http://localhost:5007"
echo -e "• Notification Service: http://localhost:5008"
echo -e "• File Storage Service: http://localhost:5009"
echo -e "• GraphQL Mesh: http://localhost:4000"
echo -e "• n8n: http://localhost:5678"
echo -e "• Frontend: http://localhost:3000"