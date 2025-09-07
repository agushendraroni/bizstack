// Auth API Service
import { graphqlClient } from '../../services/api/graphqlClient';
import { LOGIN_MUTATION } from '../../services/api/queries';

class AuthAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Login user
  async login(credentials) {
    try {
      const response = await this.client.mutate(LOGIN_MUTATION, { loginRequest: credentials });
      
      const loginData = response.AuthService_postAuthControllerLogin;
      
      if (loginData.isSuccess && loginData.data && loginData.data.accessToken) {
        // Store tokens
        localStorage.setItem('accessToken', loginData.data.accessToken);
        localStorage.setItem('refreshToken', loginData.data.refreshToken);
        
        // Store user info
        const user = {
          id: loginData.data.userId,
          username: loginData.data.username,
          companyId: loginData.data.companyId,
          companyCode: loginData.data.companyCode,
          companyName: loginData.data.companyName,
          tenantId: loginData.data.tenantId,
          roles: loginData.data.roles || []
        };
        localStorage.setItem('user', JSON.stringify(user));
        localStorage.setItem('companyCode', loginData.data.companyCode);
        localStorage.setItem('tenantId', loginData.data.tenantId);
        localStorage.setItem('userId', loginData.data.userId);
        
        // Update client token
        this.client.setToken(loginData.data.accessToken);
        
        return {
          success: true,
          data: loginData.data,
          message: 'Login successful'
        };
      }
      
      return {
        success: false,
        message: loginData.message || 'Login failed'
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Login failed'
      };
    }
  }

  // Register user (placeholder - implement when needed)
  async register(userData) {
    return {
      success: false,
      message: 'Registration not implemented yet'
    };
  }

  // Get user roles (placeholder - implement when needed)
  async getRoles() {
    return {
      success: true,
      data: ['Admin', 'User', 'Manager'],
      message: 'Roles retrieved successfully'
    };
  }

  // Logout user
  logout() {
    this.client.setToken(null);
    localStorage.removeItem('user');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('companyCode');
    localStorage.removeItem('tenantId');
    localStorage.removeItem('userId');
    
    // Redirect to company selector
    window.location.href = '/company';
    
    return {
      success: true,
      message: 'Logged out successfully'
    };
  }

  // Check if user is authenticated
  isAuthenticated() {
    const token = localStorage.getItem('accessToken');
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
    return localStorage.getItem('accessToken');
  }

  // Get tenant ID
  getTenantId() {
    return localStorage.getItem('tenantId');
  }
}

export default new AuthAPI();
