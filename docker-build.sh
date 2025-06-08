#!/bin/bash

echo "Build options:"
echo "1) Build all (frontend + backend)"
echo "2) Build frontend only"
echo "3) Build backend only (auth-service, user-service, graphql-mesh)"
echo "4) Skip build, just run containers"
read -p "Choose build option [1-4]: " build_option

build_frontend() {
  echo "Building frontend..."
  docker buildx build -t frameworkx-frontend ./frontend --load
  echo "✅ Built frontend"
}

build_backend() {
  echo "Building backend services..."

  docker buildx build -t frameworkx-auth-service ./services/auth-service --load
  echo "✅ Built auth-service"

  docker buildx build -t frameworkx-user-service ./services/user-service --load
  echo "✅ Built user-service"

  docker buildx build -t frameworkx-graphql-mesh ./services/graphql-mesh --load
  echo "✅ Built graphql-mesh"
}

case $build_option in
  1)
    build_backend
    build_frontend
    ;;
  2)
    build_frontend
    ;;
  3)
    build_backend
    ;;
  4)
    echo "Skipping build, just starting containers..."
    ;;
  *)
    echo "Invalid option. Skipping build."
    ;;
esac

echo "Starting containers..."
docker-compose up -d

sleep 5

get_port() {
  local service=$1
  local container_port=$2
  docker-compose port "$service" "$container_port" | cut -d: -f2
}

auth_port=$(get_port auth-service 80)
user_port=$(get_port user-service 80)
mesh_port=$(get_port graphql-mesh 4000)
n8n_port=$(get_port n8n 5678)
frontend_port=$(get_port frontend 80)

echo ""
echo "Service URLs (dinamis):"
echo "- Auth Service:       http://localhost:$auth_port/swagger/index.html"
echo "- User Service:       http://localhost:$user_port/swagger/index.html"
echo "- GraphQL Mesh:       http://localhost:$mesh_port"
echo "- n8n Workflow:       http://localhost:$n8n_port"
echo "- Frontend App:       http://localhost:$frontend_port"
echo ""
echo "Done."
