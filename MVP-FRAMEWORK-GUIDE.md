# 🚀 BizStack MVP Framework - Complete Guide

## 📋 Overview
BizStack adalah framework MVP yang siap pakai untuk berbagai model bisnis. Framework ini menyediakan 9 microservices yang terintegrasi dengan GraphQL API Gateway dan n8n workflow automation.

## 🏗️ Architecture

### Backend Services (9 Services)
1. **auth-service** (5001) - Authentication & Authorization
2. **user-service** (5002) - User Management & Profiles  
3. **organization-service** (5003) - Company & Organization Data
4. **product-service** (5004) - Product & Inventory Management
5. **customer-service** (5005) - Customer Relationship Management
6. **transaction-service** (5006) - Sales & Transaction Processing
7. **report-service** (5007) - Business Analytics & Reports
8. **notification-service** (5008) - Email, SMS, WhatsApp Notifications
9. **file-storage-service** (5009) - File Upload & Management

### Integration Layer
- **GraphQL Mesh** (4000) - Unified API Gateway
- **n8n** (5678) - Workflow Automation Engine
- **PostgreSQL** - Database Backend

## 🎯 Business Models Supported

### 1. Retail/UMKM
- **Products**: Inventory management, stock alerts, pricing
- **Customers**: Loyalty programs, purchase history
- **Transactions**: POS integration, payment processing
- **Reports**: Sales analytics, profit margins
- **Automation**: Reorder alerts, customer notifications

### 2. Restaurant/F&B
- **Products**: Menu items, ingredients, recipes
- **Customers**: Reservations, delivery tracking
- **Transactions**: Order management, kitchen workflow
- **Reports**: Popular dishes, revenue analysis
- **Automation**: Order notifications, inventory alerts

### 3. Service Business
- **Products**: Service packages, equipment
- **Customers**: Service history, maintenance schedules
- **Transactions**: Service bookings, billing
- **Reports**: Service performance, customer satisfaction
- **Automation**: Appointment reminders, follow-ups

### 4. Education/Training
- **Products**: Courses, materials, certifications
- **Customers**: Students, progress tracking
- **Transactions**: Enrollment, payments
- **Reports**: Learning analytics, completion rates
- **Automation**: Course reminders, progress updates

## 🔗 GraphQL Integration

### Unified API Access
All 9 services accessible through single GraphQL endpoint:
```
http://localhost:4000/graphql
```

### Sample Queries

#### Dashboard Data
```graphql
query Dashboard {
  ReportService_getReportsControllerGetDashboard {
    data {
      todaySales
      todayOrders
      totalCustomers
      lowStockProducts
    }
  }
}
```

#### Create Customer
```graphql
mutation CreateCustomer($input: CustomerService_CreateCustomerDto!) {
  CustomerService_postCustomersControllerCreateCustomer(createCustomerDto: $input) {
    data {
      id
      name
      email
    }
  }
}
```

#### Process Sale
```graphql
mutation ProcessSale($orderInput: TransactionService_CreateOrderDto!) {
  TransactionService_postOrdersControllerCreateOrder(createOrderDto: $orderInput) {
    data {
      id
      total
      status
    }
  }
}
```

## 🔄 n8n Workflow Automation

### Pre-built Workflows

#### 1. Customer Onboarding
- **Trigger**: New customer registration
- **Actions**: 
  - Create customer record
  - Send welcome email
  - Setup user account
  - Log activity

#### 2. Sales Processing
- **Trigger**: New order created
- **Actions**:
  - Process payment
  - Update inventory
  - Send receipt
  - Update customer stats

#### 3. Inventory Management
- **Trigger**: Daily schedule (9 AM)
- **Actions**:
  - Check low stock items
  - Send alerts to managers
  - Generate reorder suggestions

#### 4. Business Reports
- **Trigger**: Daily schedule (6 PM)
- **Actions**:
  - Generate sales summary
  - Compile customer metrics
  - Send report to management

## 🚀 Quick Start

### 1. Start All Services
```bash
./docker-build.sh
```

### 2. Verify Setup
```bash
./setup-mvp-framework.sh
```

### 3. Access Interfaces
- **GraphQL Playground**: http://localhost:4000
- **n8n Workflows**: http://localhost:5678 (admin/admin123)
- **Service APIs**: http://localhost:500X (X = 1-9)

## 📊 Service Endpoints

### Authentication
```
POST /api/auth/login
POST /api/auth/register
POST /api/auth/refresh
```

### Customer Management
```
GET /api/customers
POST /api/customers
PUT /api/customers/{id}
GET /api/customers/vip
```

### Product Management
```
GET /api/products
POST /api/products
PATCH /api/products/{id}/stock
GET /api/products/low-stock
```

### Transaction Processing
```
GET /api/orders
POST /api/orders
PATCH /api/orders/{id}/confirm
GET /api/orders/customer/{id}
```

### Notifications
```
POST /api/notifications/email
POST /api/notifications/sms
POST /api/notifications/whatsapp
```

### Reports & Analytics
```
GET /api/reports/dashboard
GET /api/reports/sales
GET /api/reports/customers
GET /api/reports/products
```

## 🔧 Customization Guide

### Adding New Business Logic
1. Extend existing services with new endpoints
2. Update GraphQL Mesh configuration
3. Create corresponding n8n workflows
4. Add business-specific validations

### Database Schema Extensions
1. Add migrations to respective services
2. Update DTOs and models
3. Refresh GraphQL schema
4. Test integration

### Custom Workflows
1. Access n8n at http://localhost:5678
2. Create new workflow using visual editor
3. Connect to BizStack services via HTTP requests
4. Schedule or trigger-based execution

## 🎯 Production Deployment

### Environment Setup
1. Configure production database connections
2. Setup SSL certificates
3. Configure environment variables
4. Setup monitoring and logging

### Scaling Considerations
1. Load balancing for high traffic
2. Database optimization
3. Caching strategies
4. Service mesh for complex deployments

## 📈 Business Benefits

### For Small Businesses
- **Rapid MVP Development**: Launch in days, not months
- **Cost Effective**: Open source, no licensing fees
- **Scalable**: Grow from startup to enterprise
- **Flexible**: Adapt to any business model

### For Developers
- **Modern Architecture**: Microservices + GraphQL + Automation
- **Developer Friendly**: Well documented APIs
- **Extensible**: Easy to add new features
- **Production Ready**: Built-in security and monitoring

## 🔒 Security Features
- JWT-based authentication
- Role-based access control
- Input validation and sanitization
- Audit logging
- CORS protection

## 📞 Support & Community
- Documentation: Complete API documentation
- Examples: Sample implementations for common use cases
- Community: Active development and support
- Updates: Regular feature updates and security patches

---

**BizStack MVP Framework - Empowering businesses with modern technology** 🚀