# 🔄 BizStack API Versioning Guide

## Overview
BizStack implements consistent API versioning across all 10 microservices using ASP.NET Core API Versioning with v1.0 as the current version.

## 🎯 API Versioning Pattern

### Supported URL Formats
```bash
# Default route (backwards compatible)
GET http://localhost:5001/api/roles

# Versioned route (recommended)
GET http://localhost:5001/api/v1.0/roles

# Query parameter versioning
GET http://localhost:5001/api/roles?version=1.0

# Header versioning
GET http://localhost:5001/api/roles
X-Version: 1.0
```

## 📊 Service Endpoints

### Authentication Service (Port 5001)
```bash
# Roles Management
GET    /api/v1.0/roles
POST   /api/v1.0/roles
PUT    /api/v1.0/roles/{id}
DELETE /api/v1.0/roles/{id}

# Authentication
POST   /api/v1.0/auth/login
POST   /api/v1.0/auth/register
```

### User Service (Port 5002)
```bash
# User Management
GET    /api/v1.0/users
POST   /api/v1.0/users
PUT    /api/v1.0/users/{id}
DELETE /api/v1.0/users/{id}

# User Profiles
GET    /api/v1.0/userprofiles
POST   /api/v1.0/userprofiles
```

### Organization Service (Port 5003)
```bash
# Company Management
GET    /api/v1.0/companies
POST   /api/v1.0/companies
PUT    /api/v1.0/companies/{id}

# Branches
GET    /api/v1.0/branches
POST   /api/v1.0/branches
```

### Product Service (Port 5004)
```bash
# Product Management
GET    /api/v1.0/products
POST   /api/v1.0/products
PUT    /api/v1.0/products/{id}

# Categories
GET    /api/v1.0/categories
POST   /api/v1.0/categories
```

### Customer Service (Port 5005)
```bash
# Customer Management
GET    /api/v1.0/customers
POST   /api/v1.0/customers
PUT    /api/v1.0/customers/{id}

# Customer Groups
GET    /api/v1.0/customergroups
POST   /api/v1.0/customergroups
```

### Transaction Service (Port 5006)
```bash
# Order Management
GET    /api/v1.0/orders
POST   /api/v1.0/orders
PUT    /api/v1.0/orders/{id}

# Payments
GET    /api/v1.0/payments
POST   /api/v1.0/payments
```

### Report Service (Port 5007)
```bash
# Business Reports
GET    /api/v1.0/reports/dashboard
GET    /api/v1.0/reports/sales
GET    /api/v1.0/reports/products
GET    /api/v1.0/reports/customers
```

### Notification Service (Port 5008)
```bash
# Notifications
GET    /api/v1.0/notifications
POST   /api/v1.0/notifications
PUT    /api/v1.0/notifications/{id}
```

### File Storage Service (Port 5009)
```bash
# File Management
GET    /api/v1.0/files
POST   /api/v1.0/files/upload
DELETE /api/v1.0/files/{id}
```

### Settings Service (Port 5010)
```bash
# Menu Items
GET    /api/v1.0/menuitems
POST   /api/v1.0/menuitems
PUT    /api/v1.0/menuitems/{id}

# System Settings
GET    /api/v1.0/systemsettings
POST   /api/v1.0/systemsettings
```

## 🔍 Swagger Documentation

Each service provides interactive Swagger UI documentation:

```bash
# Access Swagger UI for each service
http://localhost:5001/  # Auth Service
http://localhost:5002/  # User Service
http://localhost:5003/  # Organization Service
http://localhost:5004/  # Product Service
http://localhost:5005/  # Customer Service
http://localhost:5006/  # Transaction Service
http://localhost:5007/  # Report Service
http://localhost:5008/  # Notification Service
http://localhost:5009/  # File Storage Service
http://localhost:5010/  # Settings Service
```

## 🧪 Testing API Versioning

### Quick Test Script
```bash
#!/bin/bash
echo "🔍 Testing API Versioning:"

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

for service in "${services[@]}"; do
  IFS=':' read -r name port endpoint <<< "$service"
  echo -n "$name-service: "
  status=$(curl -s -o /dev/null -w "%{http_code}" "http://localhost:$port/api/v1.0/$endpoint")
  if [ "$status" = "200" ]; then
    echo "✅ Working"
  else
    echo "❌ Failed ($status)"
  fi
done
```

### Individual Service Tests
```bash
# Test Auth Service
curl -X GET http://localhost:5001/api/v1.0/roles

# Test Customer Service  
curl -X GET http://localhost:5005/api/v1.0/customers

# Test Product Service
curl -X GET http://localhost:5004/api/v1.0/products

# Test Report Service Dashboard
curl -X GET http://localhost:5007/api/v1.0/reports/dashboard
```

## 🔗 GraphQL Integration

GraphQL Mesh automatically integrates all versioned APIs:

```graphql
# Access through GraphQL (unified endpoint)
query GetAllServices {
  # Auth Service
  AuthService_getRolesControllerGetAllRoles {
    data
    isSuccess
  }
  
  # Customer Service  
  CustomerService_getCustomersControllerGetAllCustomers {
    data
    isSuccess
  }
  
  # Product Service
  ProductService_getProductsControllerGetAllProducts {
    data
    isSuccess
  }
}
```

## 🚀 Future Versioning

When creating new API versions:

1. **Add new version**: `[ApiVersion("2.0")]`
2. **Update routes**: `[Route("api/v{version:apiVersion}/[controller]")]`
3. **Maintain backwards compatibility**: Keep v1.0 endpoints
4. **Update Swagger**: Add new version documentation
5. **Update GraphQL**: Extend schema for new endpoints

## 🔒 Security & Headers

All versioned endpoints support:
- JWT Authentication
- CORS configuration
- Request/Response logging
- Rate limiting (when configured)
- Health checks at `/health`

## 📚 Best Practices

1. **Always use versioned endpoints** in production
2. **Test both default and versioned routes** during development  
3. **Monitor API usage** through health checks
4. **Document API changes** in changelog
5. **Use GraphQL** for complex queries across services