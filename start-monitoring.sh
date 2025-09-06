#!/bin/bash

# ==============================
# BizStack Monitoring Setup
# ==============================

YELLOW='\033[1;33m'
GREEN='\033[1;32m'
RED='\033[1;31m'
NC='\033[0m'

echo -e "${YELLOW}🔍 Starting BizStack Monitoring Stack...${NC}"

# Check if Docker is running
if ! docker info >/dev/null 2>&1; then
    echo -e "${RED}❌ Docker is not running. Please start Docker first.${NC}"
    exit 1
fi

echo -e "${YELLOW}📊 Starting monitoring services...${NC}"

# Start monitoring stack
docker-compose -f docker-compose.monitoring.yml up -d

# Wait for services to start
echo -e "${YELLOW}⏳ Waiting for services to start...${NC}"
sleep 10

# Check service status
echo -e "${YELLOW}🔍 Checking monitoring services...${NC}"

SERVICES=("prometheus:9090" "grafana:3001" "uptime-kuma:3002" "node-exporter:9100" "cadvisor:8080")

for service_port in "${SERVICES[@]}"; do
    IFS=':' read -r service port <<< "$service_port"
    if curl -s "http://localhost:$port" >/dev/null 2>&1; then
        echo -e "${GREEN}✅ $service (http://localhost:$port)${NC}"
    else
        echo -e "${RED}❌ $service (http://localhost:$port)${NC}"
    fi
done

echo -e "\n${GREEN}🎉 Monitoring stack started successfully!${NC}"
echo -e "\n${YELLOW}📋 Access URLs:${NC}"
echo -e "• Simple Monitor: file://$(pwd)/monitoring/web/index.html"
echo -e "• Prometheus: http://localhost:9090"
echo -e "• Grafana: http://localhost:3001 (admin/admin123)"
echo -e "• Uptime Kuma: http://localhost:3002"
echo -e "• Node Exporter: http://localhost:9100"
echo -e "• cAdvisor: http://localhost:8080"
echo -e "• React Frontend: http://localhost:3000"

echo -e "\n${YELLOW}🔧 Setup Instructions:${NC}"
echo -e "1. Open Simple Monitor in browser for quick overview"
echo -e "2. Configure Uptime Kuma monitors for each service"
echo -e "3. Import Grafana dashboards for detailed metrics"
echo -e "4. Set up alerts in Prometheus/Grafana"