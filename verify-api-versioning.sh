#!/bin/bash

echo "🔍 BizStack API Versioning Verification"
echo "======================================"
echo

# Test all services API versioning
services=(
  "auth:5001:roles"
  "user:5002:users" 
  "organization:5003:companies"
  "product:5004:products"
  "customer:5005:customers"
  "transaction:5006:orders"
  "report:5007:reports/dashboard"
  "notification:5008:notifications"
  "file-storage:5009:files"
  "settings:5010:menuitems"
)

echo "📊 API Versioning Status:"
echo
working=0
total=${#services[@]}

for service in "${services[@]}"; do
  IFS=':' read -r name port endpoint <<< "$service"
  printf "%-20s " "$name-service:"
  
  status=$(curl -s -o /dev/null -w "%{http_code}" "http://localhost:$port/api/v1.0/$endpoint" 2>/dev/null)
  
  if [ "$status" = "200" ]; then
    echo "✅ Working (v1.0)"
    ((working++))
  elif [ "$status" = "000" ]; then
    echo "❌ Not Running"
  else
    echo "⚠️  Response: $status"
  fi
done

echo
echo "🌐 Swagger UI Status:"
echo

for service in "${services[@]}"; do
  IFS=':' read -r name port endpoint <<< "$service"
  printf "%-20s " "$name-service:"
  
  status=$(curl -s -o /dev/null -w "%{http_code}" "http://localhost:$port/" 2>/dev/null)
  
  if [ "$status" = "301" ] || [ "$status" = "200" ]; then
    echo "✅ Accessible"
  else
    echo "❌ Failed ($status)"
  fi
done

echo
echo "🔗 GraphQL Mesh Status:"
printf "%-20s " "GraphQL Gateway:"
graphql_status=$(curl -s -o /dev/null -w "%{http_code}" "http://localhost:4000/graphql" 2>/dev/null)
if [ "$graphql_status" = "200" ]; then
  echo "✅ Working"
else
  echo "❌ Failed ($graphql_status)"
fi

echo
echo "📈 Summary:"
echo "Working Services: $working/$total"
echo "Success Rate: $(( working * 100 / total ))%"

if [ $working -eq $total ]; then
  echo "🎉 All services are working with API versioning!"
else
  echo "⚠️  Some services need attention"
fi

echo
echo "🔗 Quick Access URLs:"
echo "- GraphQL Playground: http://localhost:4000"
echo "- Auth Service Swagger: http://localhost:5001/"
echo "- Customer Service Swagger: http://localhost:5005/"
echo "- Product Service Swagger: http://localhost:5004/"
echo
