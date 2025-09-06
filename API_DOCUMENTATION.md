# BizStack API Documentation

## 🚀 Working Services (4/9)

### ✅ Auth Service (Port 5001)
**Base URL:** `http://localhost:5001`

#### Endpoints:
- `GET /health` - Health check
- `GET /api/Roles` - Get all roles
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

#### Example Usage:
```bash
# Health Check
curl http://localhost:5001/health

# Get Roles
curl http://localhost:5001/api/Roles

# Login
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "password123"}'
```

---

### ✅ User Service (Port 5002)
**Base URL:** `http://localhost:5002`

#### Endpoints:
- `GET /health` - Health check
- `GET /api/Users` - Get all users
- `POST /api/Users` - Create new user
- `PUT /api/Users/{id}` - Update user
- `DELETE /api/Users/{id}` - Delete user

#### Example Usage:
```bash
# Get Users
curl http://localhost:5002/api/Users

# Create User
curl -X POST http://localhost:5002/api/Users \
  -H "Content-Type: application/json" \
  -d '{
    "username": "newuser",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

---

### ✅ Product Service (Port 5004)
**Base URL:** `http://localhost:5004`

#### Endpoints:
- `GET /health` - Health check
- `GET /api/Products` - Get all products
- `POST /api/Products` - Create new product
- `PUT /api/Products/{id}` - Update product
- `DELETE /api/Products/{id}` - Delete product
- `GET /api/Categories` - Get all categories
- `POST /api/Categories` - Create new category

#### Example Usage:
```bash
# Get Products
curl http://localhost:5004/api/Products

# Create Product
curl -X POST http://localhost:5004/api/Products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Sample Product",
    "code": "SP001",
    "description": "Sample product description",
    "price": 99.99,
    "stock": 100,
    "unit": "pcs"
  }'
```

---

### ✅ Transaction Service (Port 5006)
**Base URL:** `http://localhost:5006`

#### Endpoints:
- `GET /health` - Health check
- `GET /api/Orders` - Get all orders
- `POST /api/Orders` - Create new order
- `PUT /api/Orders/{id}` - Update order
- `DELETE /api/Orders/{id}` - Delete order

#### Example Usage:
```bash
# Get Orders
curl http://localhost:5006/api/Orders

# Create Order
curl -X POST http://localhost:5006/api/Orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "customer-id",
    "items": [
      {
        "productId": "product-id",
        "quantity": 2,
        "price": 99.99
      }
    ]
  }'
```

---

## 🔄 Services Under Development (5/9)

### ❌ Organization Service (Port 5003)
- Status: Container running, API not responding
- Issue: Migration/configuration needed

### ❌ Customer Service (Port 5005)
- Status: Container running, API not responding
- Issue: Migration/configuration needed

### ❌ Report Service (Port 5007)
- Status: Container running, API not responding
- Issue: Migration/configuration needed

### ❌ Notification Service (Port 5008)
- Status: Container running, API not responding
- Issue: Migration/configuration needed

### ❌ File Storage Service (Port 5009)
- Status: Container running, API not responding
- Issue: Migration/configuration needed

---

## 🎯 Frontend Integration Ready

The 4 working services provide core functionality for:
- ✅ User Authentication & Management
- ✅ Product Catalog Management
- ✅ Order Processing & Transactions
- ✅ Role-Based Access Control

Perfect foundation for MVP frontend integration!

## 📊 Response Format

All APIs return standardized response format:
```json
{
  "isSuccess": true,
  "data": [...],
  "message": "Success message",
  "errors": []
}
```

## 🔐 Authentication

Most endpoints require JWT token in Authorization header:
```bash
curl -H "Authorization: Bearer YOUR_JWT_TOKEN" http://localhost:5001/api/endpoint
```
