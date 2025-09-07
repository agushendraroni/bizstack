// GraphQL Queries for BizStack Business Operations

// ===== DASHBOARD QUERIES =====
export const BUSINESS_DASHBOARD_QUERY = `
  query BusinessDashboard {
    ReportService_getReportsControllerGetDashboard {
      data {
        todaySales
        todayOrders
        totalCustomers
        lowStockProducts
      }
      isSuccess
      message
    }
  }
`;

// ===== PRODUCT QUERIES =====
export const GET_ALL_PRODUCTS_QUERY = `
  query GetAllProducts {
    ProductService_getProductsControllerGetAllProducts {
      data {
        id
        name
        code
        description
        price
        stock
        minStock
        isActive
        categoryId
        createdAt
      }
      isSuccess
      message
    }
  }
`;

export const GET_PRODUCT_BY_ID_QUERY = `
  query GetProductById($id: String!) {
    ProductService_getProductsControllerGetProductById(id: $id) {
      data {
        id
        name
        code
        description
        price
        stock
        minStock
        isActive
        categoryId
        createdAt
      }
      isSuccess
      message
    }
  }
`;

export const GET_LOW_STOCK_PRODUCTS_QUERY = `
  query GetLowStockProducts {
    ProductService_getProductsControllerGetLowStockProducts {
      data {
        id
        name
        code
        stock
        minStock
        price
      }
      isSuccess
      message
    }
  }
`;

// ===== CUSTOMER QUERIES =====
export const GET_ALL_CUSTOMERS_QUERY = `
  query GetAllCustomers {
    CustomerService_getCustomersControllerGetAllCustomers {
      data {
        id
        name
        email
        phone
        address
        customerType
        isActive
        createdAt
      }
      isSuccess
      message
    }
  }
`;

// ===== ORDER QUERIES =====
export const GET_ALL_ORDERS_QUERY = `
  query GetAllOrders {
    TransactionService_getOrdersControllerGetAllOrders {
      data {
        id
        orderNumber
        customerId
        status
        subTotal
        taxAmount
        discountAmount
        totalAmount
        orderDate
        createdAt
      }
      isSuccess
      message
    }
  }
`;

// ===== COMPANY QUERIES =====
export const GET_ALL_COMPANIES_QUERY = `
  query GetAllCompanies {
    OrganizationService_getCompaniesControllerGetAllCompanies {
      data {
        id
        name
        code
        description
        address
        phone
        email
        website
        isActive
        createdAt
      }
      isSuccess
      message
    }
  }
`;

export const GET_COMPANY_BY_CODE_QUERY = `
  query GetCompanyByCode($code: String!) {
    OrganizationService_getCompaniesControllerGetCompanyByCode(code: $code) {
      data {
        id
        name
        code
        description
        address
        phone
        email
        website
        isActive
      }
      isSuccess
      message
    }
  }
`;

// ===== USER QUERIES =====
export const GET_ALL_USERS_QUERY = `
  query GetAllUsers {
    UserService_getUserProfilesControllerGetProfiles {
      data {
        id
        userId
        fullName
        firstName
        lastName
        phoneNumber
        email
        avatarUrl
        bio
        dateOfBirth
        gender
        address
        city
        country
        postalCode
      }
      isSuccess
      message
    }
  }
`;

// ===== MUTATIONS =====

// Auth mutations
export const LOGIN_MUTATION = `
  mutation Login($loginRequest: AuthService_LoginRequest_Input!) {
    AuthService_postAuthControllerLogin(loginRequest: $loginRequest) {
      data {
        accessToken
        refreshToken
        expiresAt
        userId
        username
        companyId
        companyCode
        companyName
        tenantId
        roles
      }
      isSuccess
      message
    }
  }
`;

// Product mutations
export const CREATE_PRODUCT_MUTATION = `
  mutation CreateProduct($createProductDto: ProductService_CreateProductDto_Input!) {
    ProductService_postProductsControllerCreateProduct(createProductDto: $createProductDto) {
      data {
        id
        name
        code
        description
        price
        stock
        minStock
        isActive
      }
      isSuccess
      message
    }
  }
`;

export const UPDATE_PRODUCT_MUTATION = `
  mutation UpdateProduct($id: String!, $updateProductDto: ProductService_UpdateProductDto_Input!) {
    ProductService_putProductsControllerUpdateProduct(id: $id, updateProductDto: $updateProductDto) {
      data {
        id
        name
        code
        description
        price
        stock
        minStock
        isActive
      }
      isSuccess
      message
    }
  }
`;

export const DELETE_PRODUCT_MUTATION = `
  mutation DeleteProduct($id: String!) {
    ProductService_deleteProductsControllerDeleteProduct(id: $id) {
      data
      isSuccess
      message
    }
  }
`;

export const UPDATE_STOCK_MUTATION = `
  mutation UpdateStock($id: String!, $updateStockDto: ProductService_UpdateStockDto_Input!) {
    ProductService_patchProductsControllerUpdateStock(id: $id, updateStockDto: $updateStockDto) {
      data
      isSuccess
      message
    }
  }
`;

// Customer mutations
export const CREATE_CUSTOMER_MUTATION = `
  mutation CreateCustomer($createCustomerDto: CustomerService_CreateCustomerDto_Input!) {
    CustomerService_postCustomersControllerCreateCustomer(createCustomerDto: $createCustomerDto) {
      data {
        id
        name
        email
        phone
        address
        customerType
        isActive
      }
      isSuccess
      message
    }
  }
`;

// Order mutations
export const CREATE_ORDER_MUTATION = `
  mutation CreateOrder($createOrderDto: TransactionService_CreateOrderDto_Input!) {
    TransactionService_postOrdersControllerCreateOrder(createOrderDto: $createOrderDto) {
      data {
        id
        orderNumber
        customerId
        status
        subTotal
        taxAmount
        discountAmount
        totalAmount
        orderDate
      }
      isSuccess
      message
    }
  }
`;

// Company mutations
export const CREATE_COMPANY_MUTATION = `
  mutation CreateCompany($createCompanyDto: OrganizationService_CreateCompanyDto_Input!) {
    OrganizationService_postCompaniesControllerCreateCompany(createCompanyDto: $createCompanyDto) {
      data {
        id
        name
        code
        description
        address
        phone
        email
        website
        isActive
      }
      isSuccess
      message
    }
  }
`;

export default {
  // Queries
  BUSINESS_DASHBOARD_QUERY,
  GET_ALL_PRODUCTS_QUERY,
  GET_PRODUCT_BY_ID_QUERY,
  GET_LOW_STOCK_PRODUCTS_QUERY,
  GET_ALL_CUSTOMERS_QUERY,
  GET_ALL_ORDERS_QUERY,
  GET_ALL_COMPANIES_QUERY,
  GET_COMPANY_BY_CODE_QUERY,
  GET_ALL_USERS_QUERY,
  
  // Mutations
  LOGIN_MUTATION,
  CREATE_PRODUCT_MUTATION,
  UPDATE_PRODUCT_MUTATION,
  DELETE_PRODUCT_MUTATION,
  UPDATE_STOCK_MUTATION,
  CREATE_CUSTOMER_MUTATION,
  CREATE_ORDER_MUTATION,
  CREATE_COMPANY_MUTATION
};