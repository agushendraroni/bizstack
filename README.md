# 🚀 BizStack - Complete MVP Framework

**BizStack** adalah framework MVP lengkap yang siap pakai untuk membangun aplikasi bisnis dalam hitungan hari, bukan bulan. Dirancang modular, extensible, dan production-ready dari hari pertama.

## ✨ Fitur Utama

- 🏗️ **10 Microservices** terintegrasi penuh dengan API versioning
- 🔗 **GraphQL API Gateway** untuk unified access
- 🔄 **n8n Workflow Automation** untuk business process
- 🛡️ **JWT Authentication** & role-based access
- 📊 **Real-time Dashboard** & business analytics
- 📱 **Multi-platform** support (Web, Mobile, API)
- 🎯 **Business-agnostic** - cocok untuk semua model bisnis
- 🔄 **API Versioning** - Consistent v1.0 across all services

## 🚀 Quick Start

### 1. Clone & Setup
```bash
git clone https://github.com/agushendraroni/BizStack.git
cd BizStack
```

### 2. Start All Services
```bash
# Setup database (if needed)
./setup-database.sh

# Start all services
docker-compose up -d

# Verify setup
./setup-mvp-framework.sh
```

### 3. Access Framework
- **GraphQL API**: http://localhost:4000
- **n8n Workflows**: http://localhost:5678 (admin/admin123)
- **Service APIs**: http://localhost:500X (X=1-10)
- **Swagger UI**: http://localhost:500X/ (for each service)
- **API Endpoints**: http://localhost:500X/api/v1.0/{controller}

## 🏗️ Architecture Overview

### Backend Services (Production Ready)
| Service | Port | API Version | Status | Description |
|---------|------|-------------|--------|-------------|
| **auth-service** | 5001 | v1.0 | ✅ | JWT Authentication & Authorization |
| **user-service** | 5002 | v1.0 | ✅ | User Management & Profiles |
| **organization-service** | 5003 | v1.0 | ✅ | Company & Organization Data |
| **product-service** | 5004 | v1.0 | ✅ | Product & Inventory Management |
| **customer-service** | 5005 | v1.0 | ✅ | Customer Relationship Management |
| **transaction-service** | 5006 | v1.0 | ✅ | Sales & Transaction Processing |
| **report-service** | 5007 | v1.0 | ✅ | Business Analytics & Reports |
| **notification-service** | 5008 | v1.0 | ✅ | Email, SMS, WhatsApp Notifications |
| **file-storage-service** | 5009 | v1.0 | ✅ | File Upload & Management |
| **settings-service** | 5010 | v1.0 | ✅ | System Settings & Configuration |

### Integration Layer
- **GraphQL Mesh** (4000) - Unified API Gateway
- **n8n** (5678) - Workflow Automation Engine
- **PostgreSQL** - Database Backend
- **React Frontend** (3000) - Admin Dashboard

## 🎯 Business Models Supported

### 🏪 Retail/UMKM
- Inventory management, POS integration
- Customer loyalty programs
- Sales analytics & reporting
- Automated reorder alerts

### 🍽️ Restaurant/F&B
- Menu & ingredient management
- Order processing & kitchen workflow
- Delivery tracking & customer notifications
- Popular dishes analytics

### 🔧 Service Business
- Service packages & equipment tracking
- Appointment booking & scheduling
- Customer service history
- Performance analytics

### 🎓 Education/Training
- Course & material management
- Student progress tracking
- Enrollment & payment processing
- Learning analytics

## 🔗 GraphQL Unified API

### Single Endpoint for Everything
```graphql
# Business Dashboard
query BusinessDashboard {
  ReportService_getReportsControllerGetDashboard {
    data {
      todaySales
      todayOrders
      totalCustomers
      lowStockProducts
    }
  }
}

# Process Sale
mutation ProcessSale($customerId: String!, $items: [SaleItemInput!]!) {
  TransactionService_postOrdersControllerCreateOrder(
    createOrderDto: {
      customerId: $customerId
      items: $items
    }
  ) {
    data { id total status }
    isSuccess
  }
}
```

### Ready-to-Use Business Queries
- Dashboard metrics & KPIs
- Sales processing & analytics
- Customer management & segmentation
- Inventory tracking & alerts
- Business reports & insights

## 🔄 n8n Business Automation

### Pre-built Workflows
1. **Customer Onboarding** - Welcome emails, account setup
2. **Sales Processing** - Order confirmation, inventory updates
3. **Inventory Alerts** - Low stock notifications, reorder suggestions
4. **Business Reports** - Daily/weekly automated reports
5. **Customer Retention** - Follow-up campaigns, loyalty programs

## 📊 Technologies Used

### Backend
- **.NET 8** (C#) - High-performance microservices
- **PostgreSQL** - Reliable database backend
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **JWT** - Secure authentication
- **Swagger** - API documentation

### Integration
- **GraphQL Mesh** - API gateway & schema stitching
- **n8n** - Visual workflow automation
- **Docker** - Containerization & deployment

### Frontend
- **React** - Modern web interface
- **Shards Dashboard** - Professional UI components

## 📚 Documentation

- **[GraphQL API Guide](GRAPHQL-API-GUIDE.md)** - Complete API documentation
- **[MVP Framework Guide](MVP-FRAMEWORK-GUIDE.md)** - Business implementation guide
- **[Business Queries](services/graphql-mesh/business-queries.graphql)** - Ready-to-use GraphQL queries
- **[n8n Workflows](n8n-workflows/)** - Automation templates

## 🚀 Production Deployment

### Environment Setup
1. Configure production database connections
2. Setup SSL certificates & domain
3. Configure environment variables
4. Setup monitoring & logging

### Scaling
- Load balancing for high traffic
- Database optimization & caching
- Microservices scaling
- CDN for static assets

## 🎯 Business Benefits

### For Businesses
- **Rapid MVP Launch** - Days, not months
- **Cost Effective** - Open source, no licensing
- **Scalable** - Grow from startup to enterprise
- **Flexible** - Adapt to any business model

### For Developers
- **Modern Architecture** - Microservices + GraphQL
- **Developer Friendly** - Well documented APIs
- **Extensible** - Easy to add features
- **Production Ready** - Built-in security & monitoring

## 🔒 Security Features

- JWT-based authentication
- Role-based access control
- Input validation & sanitization
- Audit logging & activity tracking
- CORS protection
- SQL injection prevention

## 🛠️ Development

### Adding New Features
1. Extend existing services or create new ones
2. Update GraphQL schema & resolvers
3. Add corresponding n8n workflows
4. Update documentation

### Testing
```bash
# Verify all services
./verify-swagger.sh

# Test API versioning (example)
curl http://localhost:5001/api/v1.0/roles
curl http://localhost:5005/api/v1.0/customers

# Test GraphQL API
curl -X POST http://localhost:4000/graphql \
  -H "Content-Type: application/json" \
  -d '{"query": "{ __schema { types { name } } }"}'

# Test Swagger UI
open http://localhost:5001/  # Auth Service Swagger
open http://localhost:5005/  # Customer Service Swagger
```

## 🤝 Contributing

1. Fork repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open Pull Request

## 📞 Support

- **Documentation**: Complete guides & API docs
- **Examples**: Sample implementations
- **Community**: Active development & support
- **Updates**: Regular features & security patches

## 📄 License

MIT License © [agushendraroni](https://github.com/agushendraroni)

---

**BizStack - Empowering businesses with modern technology** 🚀

*Ready-to-deploy MVP framework for any business model*
