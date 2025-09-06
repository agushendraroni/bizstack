#!/bin/bash

echo "🔍 BIZSTACK SERVICES MONITORING"
echo "==============================="
echo "$(date)"
echo ""

# Check service health
services=("5001:Auth" "5002:User" "5004:Product" "5006:Transaction")
working=0
total=${#services[@]}

for service in "${services[@]}"; do
  port=$(echo $service | cut -d: -f1)
  name=$(echo $service | cut -d: -f2)
  
  response=$(curl -s http://localhost:$port/health 2>/dev/null)
  if [ -n "$response" ]; then
    echo "✅ $name Service ($port): Online"
    ((working++))
  else
    echo "❌ $name Service ($port): Offline"
  fi
done

echo ""
echo "📊 Status: $working/$total services online"
echo "🐳 Docker containers:"
docker ps --format "table {{.Names}}\t{{.Status}}" | grep BizStack | head -5

echo ""
echo "🗄️ Database connections:"
sudo -u postgres psql -c "SELECT count(*) as active_connections FROM pg_stat_activity WHERE datname LIKE '%_db';" 2>/dev/null || echo "Database check failed"

echo ""
echo "💾 Memory usage:"
free -h | head -2
