#!/bin/bash

# BizStack Docker Build Script
# Supports soft build (restart) and hard build (rebuild)
# Usage: ./docker-build.sh [soft|hard] [service-name|all]

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Service definitions
SERVICES=(
    "auth-service"
    "user-service"
    "organization-service"
    "product-service"
    "customer-service"
    "transaction-service"
    "report-service"
    "notification-service"
    "file-storage-service"
    "graphql-mesh"
)

# Database services
DB_SERVICES=(
    "postgres"
    "n8n"
)

# Frontend services
FRONTEND_SERVICES=(
    "frontend"
)

ALL_SERVICES=("${SERVICES[@]}" "${DB_SERVICES[@]}" "${FRONTEND_SERVICES[@]}")

# Functions
print_header() {
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}🐳 BizStack Docker Build Script${NC}"
    echo -e "${BLUE}================================${NC}"
}

print_usage() {
    echo -e "${YELLOW}Usage:${NC}"
    echo "  ./docker-build.sh soft [service|all]     # Restart services (no rebuild)"
    echo "  ./docker-build.sh hard [service|all]     # Rebuild and restart services"
    echo ""
    echo -e "${YELLOW}Available Services:${NC}"
    printf "  %-25s %-25s\n" "Backend Services:" "Infrastructure:"
    for i in "${!SERVICES[@]}"; do
        if [ $i -lt 5 ]; then
            printf "  %-25s" "${SERVICES[$i]}"
            if [ $i -eq 0 ]; then echo " postgres"
            elif [ $i -eq 1 ]; then echo " n8n"
            elif [ $i -eq 2 ]; then echo " frontend"
            else echo ""
            fi
        else
            echo "  ${SERVICES[$i]}"
        fi
    done
    echo ""
    echo -e "${YELLOW}Examples:${NC}"
    echo "  ./docker-build.sh soft all              # Restart all services"
    echo "  ./docker-build.sh hard auth-service     # Rebuild auth service"
    echo "  ./docker-build.sh soft user-service     # Restart user service"
}

check_docker() {
    if ! command -v docker &> /dev/null; then
        echo -e "${RED}❌ Docker not found. Please install Docker first.${NC}"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        echo -e "${RED}❌ Docker Compose not found. Please install Docker Compose first.${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}✅ Docker and Docker Compose found${NC}"
}

get_service_ports() {
    local service=$1
    local ports=()
    
    case $service in
        "auth-service") ports=("5001") ;;
        "user-service") ports=("5002") ;;
        "organization-service") ports=("5003") ;;
        "product-service") ports=("5004") ;;
        "customer-service") ports=("5005") ;;
        "transaction-service") ports=("5006") ;;
        "report-service") ports=("5007") ;;
        "notification-service") ports=("5008") ;;
        "file-storage-service") ports=("5009") ;;
        "settings-service") ports=("5010") ;;
        "graphql-mesh") ports=("4000") ;;
        "frontend") ports=("3000") ;;
        "postgres") ports=("5432") ;;
        "n8n") ports=("5678" "5679") ;;
        "all") ports=("3000" "4000" "5001" "5002" "5003" "5004" "5005" "5006" "5007" "5008" "5009" "5010" "5678" "5679") ;;
        *) ports=() ;;
    esac
    
    echo "${ports[@]}"
}

check_port_conflicts() {
    local service=$1
    echo -e "${YELLOW}🔍 Checking for port conflicts for $service...${NC}"
    
    # Get ports for the specific service
    local ports=($(get_service_ports "$service"))
    local conflicts_found=false
    
    if [ ${#ports[@]} -eq 0 ]; then
        echo -e "${GREEN}✅ No ports to check for $service${NC}"
        return 0
    fi
    
    for port in "${ports[@]}"; do
        if netstat -tulpn 2>/dev/null | grep -q ":$port "; then
            echo -e "${YELLOW}⚠️  Port $port is in use by $service${NC}"
            
            # Try to kill processes using the port
            if command -v fuser &> /dev/null; then
                echo -e "${YELLOW}🔧 Attempting to free port $port...${NC}"
                fuser -k ${port}/tcp 2>/dev/null || true
                sleep 2
                
                # Check if port is still in use
                if netstat -tulpn 2>/dev/null | grep -q ":$port "; then
                    echo -e "${RED}❌ Could not free port $port${NC}"
                    conflicts_found=true
                else
                    echo -e "${GREEN}✅ Port $port freed${NC}"
                fi
            else
                conflicts_found=true
            fi
        fi
    done
    
    if [ "$conflicts_found" = true ]; then
        if [[ "$service" == "all" ]]; then
            echo -e "${YELLOW}⚠️  Some port conflicts detected. Stopping all containers first...${NC}"
            docker-compose down 2>/dev/null || true
        else
            echo -e "${YELLOW}⚠️  Port conflicts detected for $service. Stopping service container...${NC}"
            docker-compose stop "$service" 2>/dev/null || true
        fi
        sleep 3
    fi
    
    echo -e "${GREEN}✅ Port conflict check completed for $service${NC}"
}

validate_service() {
    local service=$1
    if [[ "$service" == "all" ]]; then
        return 0
    fi
    
    for s in "${ALL_SERVICES[@]}"; do
        if [[ "$s" == "$service" ]]; then
            return 0
        fi
    done
    
    echo -e "${RED}❌ Invalid service: $service${NC}"
    echo -e "${YELLOW}Available services: ${ALL_SERVICES[*]} all${NC}"
    exit 1
}

soft_build() {
    local service=$1
    echo -e "${BLUE}🔄 Soft Build (Restart): $service${NC}"
    
    if [[ "$service" == "all" ]]; then
        echo -e "${YELLOW}📋 Restarting all services...${NC}"
        docker-compose restart
        echo -e "${GREEN}✅ All services restarted${NC}"
    else
        echo -e "${YELLOW}📋 Restarting $service...${NC}"
        docker-compose restart "$service"
        echo -e "${GREEN}✅ $service restarted${NC}"
    fi
}

hard_build() {
    local service=$1
    echo -e "${BLUE}🔨 Hard Build (Rebuild): $service${NC}"
    
    if [[ "$service" == "all" ]]; then
        echo -e "${YELLOW}📋 Stopping all services...${NC}"
        docker-compose down
        
        echo -e "${YELLOW}📋 Removing old images...${NC}"
        docker-compose build --no-cache
        
        echo -e "${YELLOW}📋 Starting all services...${NC}"
        docker-compose up -d
        
        echo -e "${GREEN}✅ All services rebuilt and started${NC}"
    else
        echo -e "${YELLOW}📋 Stopping $service...${NC}"
        docker-compose stop "$service"
        
        echo -e "${YELLOW}📋 Removing old image for $service...${NC}"
        docker-compose build --no-cache "$service"
        
        echo -e "${YELLOW}📋 Starting $service...${NC}"
        docker-compose up -d "$service"
        
        echo -e "${GREEN}✅ $service rebuilt and started${NC}"
    fi
}

check_service_health() {
    local service=$1
    local port=""
    
    # Map service to port
    case $service in
        "auth-service") port="5001" ;;
        "user-service") port="5002" ;;
        "organization-service") port="5003" ;;
        "product-service") port="5004" ;;
        "customer-service") port="5005" ;;
        "transaction-service") port="5006" ;;
        "report-service") port="5007" ;;
        "notification-service") port="5008" ;;
        "file-storage-service") port="5009" ;;
        "graphql-mesh") port="4000" ;;
        "frontend") port="3000" ;;
        *) return 0 ;;
    esac
    
    if [[ -n "$port" ]]; then
        echo -e "${YELLOW}🧪 Testing $service on port $port...${NC}"
        sleep 5
        
        if curl -s "http://localhost:$port/health" > /dev/null 2>&1; then
            echo -e "${GREEN}✅ $service is healthy${NC}"
        elif curl -s "http://localhost:$port/" > /dev/null 2>&1; then
            echo -e "${GREEN}✅ $service is responding${NC}"
        else
            echo -e "${RED}❌ $service is not responding${NC}"
        fi
    fi
}

run_health_checks() {
    local service=$1
    echo -e "${BLUE}🏥 Running Health Checks${NC}"
    echo "=========================="
    
    if [[ "$service" == "all" ]]; then
        for s in "${SERVICES[@]}"; do
            check_service_health "$s"
        done
        
        # Check frontend
        check_service_health "frontend"
        
        # Summary
        echo ""
        echo -e "${BLUE}📊 Service Status Summary:${NC}"
        docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | grep BizStack
        
    else
        check_service_health "$service"
        
        echo ""
        echo -e "${BLUE}📊 $service Status:${NC}"
        docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | grep "$service" || echo "Service not found"
    fi
}

show_service_logs() {
    local service=$1
    echo -e "${BLUE}📋 Recent logs for $service:${NC}"
    docker-compose logs --tail=10 "$service" 2>/dev/null || echo "No logs available"
}

# Main script
main() {
    print_header
    
    # Check arguments
    if [[ $# -lt 1 ]]; then
        print_usage
        exit 1
    fi
    
    local build_type=$1
    local service=${2:-"all"}
    
    # Validate build type
    if [[ "$build_type" != "soft" && "$build_type" != "hard" ]]; then
        echo -e "${RED}❌ Invalid build type: $build_type${NC}"
        echo -e "${YELLOW}Use 'soft' for restart or 'hard' for rebuild${NC}"
        print_usage
        exit 1
    fi
    
    # Validate service
    validate_service "$service"
    
    # Check Docker
    check_docker
    
    # Check for port conflicts
    check_port_conflicts "$service"
    
    # Show current status
    echo -e "${BLUE}📊 Current Status:${NC}"
    docker ps --format "table {{.Names}}\t{{.Status}}" | grep BizStack | head -5
    echo ""
    
    # Execute build
    case $build_type in
        "soft")
            soft_build "$service"
            ;;
        "hard")
            hard_build "$service"
            ;;
    esac
    
    # Wait for services to start
    echo -e "${YELLOW}⏳ Waiting for services to start...${NC}"
    sleep 10
    
    # Run health checks
    run_health_checks "$service"
    
    # Show logs if single service
    if [[ "$service" != "all" ]]; then
        echo ""
        show_service_logs "$service"
    fi
    
    echo ""
    echo -e "${GREEN}🎉 Build completed successfully!${NC}"
    echo -e "${BLUE}📋 Quick Access URLs:${NC}"
    echo "  Frontend:        http://localhost:3000"
    echo "  Auth Service:    http://localhost:5001/"
    echo "  User Service:    http://localhost:5002/"
    echo "  Product Service: http://localhost:5004/"
    echo "  GraphQL Mesh:    http://localhost:4000/"
    echo "  n8n Workflows:   http://localhost:5679/ (admin/admin123)"
}

# Execute main function
main "$@"
