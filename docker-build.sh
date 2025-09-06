#!/bin/bash

# ==============================
# Colors
# ==============================
YELLOW='\033[1;33m'
GREEN='\033[1;32m'
RED='\033[1;31m'
NC='\033[0m'

# ==============================
# Service Configuration
# ==============================
FRONTEND_SERVICE=("frontend" "./frontend" 3000 "Frontend")
BACKEND_SERVICES=(
  "auth-service ./services/auth-service 5282 'Auth' /index.html"
  "user-service ./services/user-service 5283 'User' "
  "organization-service ./services/organization-service 5296 'Org' "
  "graphql-mesh ./services/graphql-mesh 4000 'GraphQL' "
)
OTHER_SERVICES=(
  "n8n ./services/n8n 5678 'n8n'"
)

# ==============================
# Helpers
# ==============================
free_port_if_used() {
  local port=$1
  local pid
  local container_id

  # Check Docker containers using the port
  container_id=$(docker ps --filter "publish=${port}" --format "{{.ID}}")
  if [[ -n "$container_id" ]]; then
    echo -e "${YELLOW}⚠ Port $port in use by Docker container $container_id. Stopping...${NC}"
    docker stop "$container_id" >/dev/null && echo -e "${GREEN}✓ Freed port $port from Docker${NC}"
  fi

  # Check regular processes
  pid=$(sudo lsof -ti tcp:$port)
  if [[ -n "$pid" ]]; then
    echo -e "${YELLOW}⚠ Port $port in use by PID $pid. Killing...${NC}"
    sudo kill -9 "$pid" && echo -e "${GREEN}✓ Freed port $port${NC}"
  fi
}

free_ports_for_build() {
  local target=$1
  if [[ "$target" == "frontend" ]]; then
    free_port_if_used "${FRONTEND_SERVICE[2]}"
  elif [[ "$target" == "backend" ]]; then
    for svc in "${BACKEND_SERVICES[@]}"; do
      set -- $svc
      free_port_if_used "$3"
    done
  elif [[ "$target" == "all" ]]; then
    free_port_if_used "${FRONTEND_SERVICE[2]}"
    for svc in "${BACKEND_SERVICES[@]}"; do
      set -- $svc
      free_port_if_used "$3"
    done
    for svc in "${OTHER_SERVICES[@]}"; do
      set -- $svc
      free_port_if_used "$3"
    done
  fi
}

free_all_service_ports() {
  free_port_if_used "${FRONTEND_SERVICE[2]}"

  for svc in "${BACKEND_SERVICES[@]}"; do
    set -- $svc
    free_port_if_used "$3"
  done

  for svc in "${OTHER_SERVICES[@]}"; do
    set -- $svc
    free_port_if_used "$3"
  done
}

build_service() {
  local name=$1
  local path=$2
  local build_context="."

  if [[ "$name" == "graphql-mesh" || "$name" == "frontend" ]]; then
    build_context="$path"
  fi

  echo -e "${NC}→ docker buildx build -t bizstack-$name -f $path/Dockerfile $build_context --load${NC}"
  docker buildx build -t "bizstack-$name" -f "$path/Dockerfile" "$build_context" --load

  if [[ $? -eq 0 ]]; then
    echo -e "${GREEN}✅ $name${NC}"
  else
    echo -e "${RED}❌ $name${NC}"
    exit 1
  fi
}

get_port() {
  local service=$1
  local container_port=$2
  for i in {1..20}; do
    port=$(docker-compose port "$service" "$container_port" 2>/dev/null | cut -d: -f2)
    [[ -n "$port" ]] && echo "$port" && return
    sleep 1
  done
  echo "N/A"
}

print_service_url() {
  local port=$1
  local label=$2
  local suffix=$3
  if [[ "$port" == "N/A" ]]; then
    echo -e "${RED}- $label: Unavailable${NC}"
  else
    echo -e "${GREEN}- $label:${NC} http://localhost:$port$suffix"
  fi
}

# ==============================
# Build Functions
# ==============================
build_frontend() {
  build_service "${FRONTEND_SERVICE[0]}" "${FRONTEND_SERVICE[1]}"
}

build_backend() {
  for svc in "${BACKEND_SERVICES[@]}"; do
    set -- $svc
    build_service "$1" "$2"
  done
}

# ==============================
# Menu
# ==============================
clear
echo -e "${YELLOW}Build options:${NC}"
echo "1) All"
echo "2) Frontend"
echo "3) Backend"
echo "4) Skip build"
read -p "Choose [1-4]: " build_option

case $build_option in
  1)
    free_ports_for_build "all"
    build_backend
    build_frontend
    ;;
  2)
    free_ports_for_build "frontend"
    build_frontend
    ;;
  3)
    free_ports_for_build "backend"
    build_backend
    ;;
  4)
    echo -e "${YELLOW}Skipping build...${NC}"
    ;;
  *)
    echo -e "${RED}Invalid option, skipping build.${NC}"
    ;;
esac

# ==============================
# Start Containers
# ==============================
echo -e "${YELLOW}Checking and freeing any used ports before starting containers...${NC}"
free_all_service_ports

echo -e "${YELLOW}Starting containers...${NC}"
docker-compose up -d

echo ""
echo -e "${YELLOW}Service URLs:${NC}"

# Backend
for svc in "${BACKEND_SERVICES[@]}"; do
  set -- $svc
  port=$(get_port "$1" "$3")
  print_service_url "$port" "$4" "$5"
done

# Frontend
port=$(get_port "${FRONTEND_SERVICE[0]}" "${FRONTEND_SERVICE[2]}")
print_service_url "$port" "${FRONTEND_SERVICE[3]}" ""

# Other
for svc in "${OTHER_SERVICES[@]}"; do
  set -- $svc
  port=$(get_port "$1" "$3")
  print_service_url "$port" "$4" ""
done

echo -e "${YELLOW}All services started.${NC}"
