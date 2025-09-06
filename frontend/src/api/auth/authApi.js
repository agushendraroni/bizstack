// Auth API Service
import { apiClients, API_ENDPOINTS } from '../../services/api/apiConfig';

class AuthAPI {
  constructor() {
    this.client = apiClients.auth;
  }

  // Login user
  async login(credentials) {
    try {
      const response = await this.client.post(API_ENDPOINTS.auth.login, credentials);
      
      if (response.isSuccess && response.data?.token) {
        // Store token
        this.client.setToken(response.data.token);
        
        // Store user info
        localStorage.setItem('user', JSON.stringify(response.data.user));
        
        return {
          success: true,
          data: response.data,
          message: 'Login successful'
        };
      }
      
      return {
        success: false,
        message: response.message || 'Login failed'
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Login failed'
      };
    }
  }

  // Register user
  async register(userData) {
    try {
      const response = await this.client.post(API_ENDPOINTS.auth.register, userData);
      
      return {
        success: response.isSuccess,
        data: response.data,
        message: response.message || (response.isSuccess ? 'Registration successful' : 'Registration failed')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Registration failed'
      };
    }
  }

  // Get user roles
  async getRoles() {
    try {
      const response = await this.client.get(API_ENDPOINTS.auth.roles);
      
      return {
        success: response.isSuccess,
        data: response.data || [],
        message: response.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch roles'
      };
    }
  }

  // Logout user
  logout() {
    this.client.setToken(null);
    localStorage.removeItem('user');
    localStorage.removeItem('authToken');
    
    return {
      success: true,
      message: 'Logged out successfully'
    };
  }

  // Check if user is authenticated
  isAuthenticated() {
    const token = localStorage.getItem('authToken');
    const user = localStorage.getItem('user');
    
    return !!(token && user);
  }

  // Get current user
  getCurrentUser() {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }

  // Get auth token
  getToken() {
    return localStorage.getItem('authToken');
  }
}

export default new AuthAPI();
