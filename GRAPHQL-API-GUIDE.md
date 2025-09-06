# 🚀 BizStack GraphQL API - Complete Business Framework

## 📋 Overview
Unified GraphQL API yang menggabungkan 9 microservices menjadi satu endpoint powerful untuk semua kebutuhan bisnis.

## 🔗 Access Points
- **GraphQL Playground**: http://localhost:4000
- **GraphQL Endpoint**: http://localhost:4000/graphql
- **Schema Explorer**: Available in playground

## 🎯 Core Business Operations

### 1. DASHBOARD - Real-time Business Metrics
```graphql
query BusinessDashboard {
  businessDashboard {
    todaySales
    totalCustomers
    lowStockCount
    pendingOrders
  }
  
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

### 2. COMPLETE BUSINESS OVERVIEW
```graphql
query CompleteBusinessOverview {
  # Company Info
  OrganizationService_getCompaniesControllerGetAllCompanies {
    data { id name code address phone email }
  }
  
  # Staff & Users
  UserService_getUsersControllerGetAllUsers {
    data { id email name role isActive }
  }
  
  # Product Inventory
  ProductService_getProductsControllerGetAllProducts {
    data { id name price stock status }
  }
  
  # Customer Base
  CustomerService_getCustomersControllerGetAllCustomers {
    data { id name email customerType totalSpent }
  }
}
```

### 3. SALES PROCESSING - Core Business Operation
```graphql
mutation ProcessQuickSale($customerId: String!, $items: [SaleItemInput!]!, $paymentMethod: String!) {
  processQuickSale(
    customerId: $customerId
    items: $items
    paymentMethod: $paymentMethod
  ) {
    success
    orderId
    message
  }
}
```

**Variables:**
```json
{
  "customerId": "customer-uuid",
  "items": [
    {
      "productId": "product-uuid",
      "quantity": 2,
      "price": 25.99
    }
  ],
  "paymentMethod": "cash"
}
```

## 📊 Business Analytics & Reports

### Sales Performance
```graphql
query BusinessReports($startDate: String, $endDate: String) {
  ReportService_getReportsControllerGetSalesReport(startDate: $startDate, endDate: $endDate) {
    data {
      date
      totalSales
      totalOrders
      averageOrderValue
    }
  }
}
```

### Inventory Management
```graphql
query InventoryStatus {
  # Low stock alerts
  ProductService_getProductsControllerGetLowStockProducts {
    data { id name stock price }
  }
  
  # All products
  ProductService_getProductsControllerGetAllProducts {
    data { id name price stock status }
  }
}
```

## 👥 Customer Management

### Create New Customer
```graphql
mutation CreateCustomer($customerData: CustomerService_CreateCustomerDto!) {
  CustomerService_postCustomersControllerCreateCustomer(createCustomerDto: $customerData) {
    data { id name email customerType }
    isSuccess
    message
  }
}
```

### Get VIP Customers
```graphql
query VIPCustomers {
  CustomerService_getCustomersControllerGetVipCustomers {
    data {
      id
      name
      totalSpent
      orderCount
    }
  }
}
```

## 📦 Product & Inventory Operations

### Update Stock
```graphql
mutation UpdateStock($productId: String!, $stockData: ProductService_UpdateStockDto!) {
  ProductService_patchProductsControllerUpdateStock(id: $productId, updateStockDto: $stockData) {
    data
    isSuccess
    message
  }
}
```

### Search Products
```graphql
query SearchProducts($searchTerm: String!) {
  ProductService_getProductsControllerSearchProducts(searchTerm: $searchTerm) {
    data {
      id
      name
      price
      stock
    }
  }
}
```

## 🔔 Notification System

### Send Business Notification
```graphql
mutation SendNotification($notificationData: NotificationService_SendNotificationDto!) {
  NotificationService_postNotificationsControllerSendNotification(sendNotificationDto: $notificationData) {
    data
    isSuccess
    message
  }
}
```

### Email Notification
```graphql
mutation SendEmail($emailData: NotificationService_SendEmailDto!) {
  NotificationService_postNotificationsControllerSendEmail(sendEmailDto: $emailData) {
    data
    isSuccess
    message
  }
}
```

## 📁 File Management

### Upload File
```graphql
mutation UploadFile($fileData: FileStorageService_UploadFileDto!) {
  FileStorageService_postFilesControllerUploadFile(uploadFileDto: $fileData) {
    data {
      fileId
      fileName
      fileUrl
      fileSize
    }
  }
}
```

### Get Files by Category
```graphql
query GetFiles($category: String) {
  FileStorageService_getFilesControllerGetFiles(category: $category) {
    data {
      id
      fileName
      category
      fileSize
      uploadedAt
      fileUrl
    }
  }
}
```

## 🔐 Authentication

### Login
```graphql
mutation Login($credentials: AuthService_LoginRequest_Input!) {
  AuthService_postAuthControllerLogin(loginRequest: $credentials) {
    data {
      token
      refreshToken
      user { id email name role }
    }
    isSuccess
    message
  }
}
```

## 🏢 Business Model Adaptations

### Retail/UMKM
```graphql
# Focus on inventory and sales
query RetailDashboard {
  businessDashboard { todaySales totalCustomers lowStockCount }
  ProductService_getProductsControllerGetLowStockProducts { data { name stock } }
  CustomerService_getCustomersControllerGetVipCustomers { data { name totalSpent } }
}
```

### Restaurant/F&B
```graphql
# Focus on orders and menu items
query RestaurantDashboard {
  TransactionService_getOrdersControllerGetAllOrders { data { id total status } }
  ProductService_getProductsControllerGetAllProducts { data { name price stock } }
}
```

### Service Business
```graphql
# Focus on customers and appointments
query ServiceDashboard {
  CustomerService_getCustomersControllerGetAllCustomers { data { name email phone } }
  TransactionService_getOrdersControllerGetAllOrders { data { customerId total status } }
}
```

### Education/Training
```graphql
# Focus on students and courses
query EducationDashboard {
  CustomerService_getCustomersControllerGetAllCustomers { data { name email } }
  ProductService_getProductsControllerGetAllProducts { data { name price } }
}
```

## 🚀 Quick Start Examples

### 1. Get Business Status
```bash
curl -X POST http://localhost:4000/graphql \
  -H "Content-Type: application/json" \
  -d '{"query": "{ businessDashboard { todaySales totalCustomers } }"}'
```

### 2. Process a Sale
```bash
curl -X POST http://localhost:4000/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation($customerId: String!, $items: [SaleItemInput!]!, $paymentMethod: String!) { processQuickSale(customerId: $customerId, items: $items, paymentMethod: $paymentMethod) { success orderId message } }",
    "variables": {
      "customerId": "customer-123",
      "items": [{"productId": "product-456", "quantity": 1, "price": 29.99}],
      "paymentMethod": "cash"
    }
  }'
```

## 📈 Performance Benefits

### Single API Endpoint
- **Reduced Network Calls**: 1 request instead of 9
- **Type Safety**: GraphQL schema validation
- **Efficient Data Loading**: Only fetch what you need
- **Real-time Updates**: Live data from all services

### Business Intelligence
- **Unified Analytics**: All business metrics in one query
- **Cross-service Insights**: Correlate data across services
- **Real-time Monitoring**: Live business dashboard
- **Automated Reporting**: Scheduled data aggregation

## 🔧 Customization

### Add Custom Business Logic
1. Extend resolvers in `resolvers/business-aggregates.js`
2. Add new types in schema
3. Update GraphQL Mesh configuration
4. Test with GraphQL Playground

### Business-Specific Queries
Create custom queries for your specific business needs by combining existing services and adding business logic in resolvers.

---

**BizStack GraphQL API - One API for All Business Needs** 🚀