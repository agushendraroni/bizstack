// API Configuration for BizStack Services
// Using GraphQL Mesh for unified API access

const GRAPHQL_ENDPOINT = process.env.REACT_APP_GRAPHQL_ENDPOINT;
const API_VERSION = process.env.REACT_APP_API_VERSION;

const API_BASE_URLS = {
  graphql: process.env.REACT_APP_GRAPHQL_ENDPOINT?.replace('/graphql', ''),
  auth: process.env.REACT_APP_AUTH_SERVICE,
  user: process.env.REACT_APP_USER_SERVICE,
  organization: process.env.REACT_APP_ORGANIZATION_SERVICE,
  product: process.env.REACT_APP_PRODUCT_SERVICE,
  customer: process.env.REACT_APP_CUSTOMER_SERVICE,
  transaction: process.env.REACT_APP_TRANSACTION_SERVICE,
  report: process.env.REACT_APP_REPORT_SERVICE,
  notification: process.env.REACT_APP_NOTIFICATION_SERVICE,
  fileStorage: process.env.REACT_APP_FILE_STORAGE_SERVICE,
  settings: process.env.REACT_APP_SETTINGS_SERVICE
};

// API Endpoints with versioning
export const API_ENDPOINTS = {
  // Auth Service
  auth: {
    login: `/api/${API_VERSION}/auth/login`,
    register: `/api/${API_VERSION}/auth/register`,
    refresh: `/api/${API_VERSION}/auth/refresh`,
    roles: `/api/${API_VERSION}/roles`
  },
  
  // User Service
  user: {
    list: `/api/${API_VERSION}/users`,
    create: `/api/${API_VERSION}/users`,
    update: (id) => `/api/${API_VERSION}/users/${id}`,
    delete: (id) => `/api/${API_VERSION}/users/${id}`,
    getById: (id) => `/api/${API_VERSION}/users/${id}`
  },
  
  // Organization Service
  organization: {
    companies: `/api/${API_VERSION}/companies`,
    createCompany: `/api/${API_VERSION}/companies`,
    updateCompany: (id) => `/api/${API_VERSION}/companies/${id}`,
    deleteCompany: (id) => `/api/${API_VERSION}/companies/${id}`
  },
  
  // Product Service
  product: {
    list: '/api/Products',
    create: '/api/Products',
    update: (id) => `/api/Products/${id}`,
    delete: (id) => `/api/Products/${id}`,
    categories: '/api/Categories'
  },
  
  // Customer Service
  customer: {
    list: '/api/Customers',
    create: '/api/Customers',
    update: (id) => `/api/Customers/${id}`,
    delete: (id) => `/api/Customers/${id}`
  },
  
  // Settings Service
  settings: {
    menuItems: '/api/MenuItems',
    menuTree: (context) => `/api/MenuItems/tree/${context}`,
    menuFlat: (context) => `/api/MenuItems/flat?context=${context}`
  }
};

// Base API client
class ApiClient {
  constructor(baseURL) {
    this.baseURL = baseURL;
    this.token = localStorage.getItem('authToken');
  }

  // Set auth token
  setToken(token) {
    this.token = token;
    if (token) {
      localStorage.setItem('authToken', token);
    } else {
      localStorage.removeItem('authToken');
    }
  }

  // Get headers
  getHeaders() {
    const headers = {
      'Content-Type': 'application/json'
    };
    
    if (this.token) {
      headers.Authorization = `Bearer ${this.token}`;
    }
    
    return headers;
  }

  // Generic request method
  async request(endpoint, options = {}) {
    const url = `${this.baseURL}${endpoint}`;
    
    const config = {
      headers: this.getHeaders(),
      ...options
    };

    try {
      const response = await fetch(url, config);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  // HTTP methods
  async get(endpoint) {
    return this.request(endpoint, { method: 'GET' });
  }

  async post(endpoint, data) {
    return this.request(endpoint, {
      method: 'POST',
      body: JSON.stringify(data)
    });
  }

  async put(endpoint, data) {
    return this.request(endpoint, {
      method: 'PUT',
      body: JSON.stringify(data)
    });
  }

  async delete(endpoint) {
    return this.request(endpoint, { method: 'DELETE' });
  }
}

// Create API clients for each service
export const apiClients = {
  auth: new ApiClient(API_BASE_URLS.auth),
  user: new ApiClient(API_BASE_URLS.user),
  organization: new ApiClient(API_BASE_URLS.organization),
  product: new ApiClient(API_BASE_URLS.product),
  customer: new ApiClient(API_BASE_URLS.customer),
  transaction: new ApiClient(API_BASE_URLS.transaction),
  report: new ApiClient(API_BASE_URLS.report),
  notification: new ApiClient(API_BASE_URLS.notification),
  fileStorage: new ApiClient(API_BASE_URLS.fileStorage),
  settings: new ApiClient(API_BASE_URLS.settings)
};

export default ApiClient;
