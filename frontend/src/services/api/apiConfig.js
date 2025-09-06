// API Configuration for BizStack Services
// Direct API access for MVP - will migrate to GraphQL later

const API_BASE_URLS = {
  auth: 'http://localhost:5001',
  user: 'http://localhost:5002', 
  organization: 'http://localhost:5003',
  product: 'http://localhost:5004',
  customer: 'http://localhost:5005',
  transaction: 'http://localhost:5006',
  report: 'http://localhost:5007',
  notification: 'http://localhost:5008',
  fileStorage: 'http://localhost:5009'
};

// API Endpoints
export const API_ENDPOINTS = {
  // Auth Service
  auth: {
    login: '/api/auth/login',
    register: '/api/auth/register',
    refresh: '/api/auth/refresh',
    roles: '/api/Roles'
  },
  
  // User Service
  user: {
    list: '/api/Users',
    create: '/api/Users',
    update: (id) => `/api/Users/${id}`,
    delete: (id) => `/api/Users/${id}`,
    getById: (id) => `/api/Users/${id}`
  },
  
  // Organization Service
  organization: {
    companies: '/api/Companies',
    createCompany: '/api/Companies',
    updateCompany: (id) => `/api/Companies/${id}`,
    deleteCompany: (id) => `/api/Companies/${id}`
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
  fileStorage: new ApiClient(API_BASE_URLS.fileStorage)
};

export default ApiClient;
