#!/bin/bash

echo "🔧 Testing Auth Service API Versioning..."

# Start the service in background
echo "Starting auth service..."
dotnet run --urls="http://localhost:5001" &
SERVICE_PID=$!

# Wait for service to start
sleep 5

echo "Testing API endpoints..."

# Test versioned endpoints
echo "1. Testing /api/v1.0/auth (versioned route):"
curl -s -o /dev/null -w "%{http_code}" http://localhost:5001/api/v1.0/auth/login

echo -e "\n2. Testing /api/auth (default route):"
curl -s -o /dev/null -w "%{http_code}" http://localhost:5001/api/auth/login

echo -e "\n3. Testing /api/v1.0/roles (versioned route):"
curl -s -o /dev/null -w "%{http_code}" http://localhost:5001/api/v1.0/roles

echo -e "\n4. Testing /api/roles (default route):"
curl -s -o /dev/null -w "%{http_code}" http://localhost:5001/api/roles

echo -e "\n5. Testing Swagger UI:"
curl -s -o /dev/null -w "%{http_code}" http://localhost:5001/swagger

echo -e "\n\n✅ API Versioning test completed!"

# Clean up
kill $SERVICE_PID 2>/dev/null