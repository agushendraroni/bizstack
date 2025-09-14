// API Client for REST endpoints
class ApiClient {
  constructor() {
    this.baseURL = `${process.env.REACT_APP_AUTH_SERVICE}/api/${process.env.REACT_APP_API_VERSION}`;
    this.token = localStorage.getItem('accessToken');
  }

  setToken(token) {
    this.token = token;
    if (token) {
      localStorage.setItem('accessToken', token);
    } else {
      localStorage.removeItem('accessToken');
    }
  }

  getHeaders() {
    const headers = {
      'Content-Type': 'application/json'
    };
    
    if (this.token) {
      headers.Authorization = `Bearer ${this.token}`;
    }

    const companyCode = localStorage.getItem('companyCode');
    const tenantId = localStorage.getItem('tenantId');
    const userId = localStorage.getItem('userId');

    if (companyCode) headers['X-Company-Code'] = companyCode;
    if (tenantId) headers['X-Tenant-Id'] = tenantId;
    if (userId) headers['X-User-Id'] = userId;
    
    return headers;
  }

  async request(endpoint, options = {}) {
    try {
      const url = `${this.baseURL}${endpoint}`;
      const config = {
        headers: this.getHeaders(),
        ...options
      };

      const response = await fetch(url, config);
      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || 'Request failed');
      }

      return data;
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

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

export const apiClient = new ApiClient();
export default ApiClient;