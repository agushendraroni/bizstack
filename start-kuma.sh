#!/bin/bash

echo "🚀 Starting Uptime Kuma..."

# Start Uptime Kuma
docker-compose -f docker-compose.monitoring-kuma-only.yml up -d

# Wait for service to start
sleep 5

# Check status
echo "✅ Checking Uptime Kuma status..."
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | grep uptime-kuma

echo ""
echo "🎉 Uptime Kuma ready!"
echo "📱 Access: http://localhost:3002"
echo ""
echo "💡 Add these monitors:"
echo "   Auth Service: http://localhost:5001/health"
echo "   User Service: http://localhost:5002/health"
echo "   GraphQL API: http://localhost:4000/graphql"
echo ""