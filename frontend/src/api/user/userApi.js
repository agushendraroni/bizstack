// User API Service
import { apiClients, API_ENDPOINTS } from '../../services/api/apiConfig';

class UserAPI {
  constructor() {
    this.client = apiClients.user;
  }

  // Get all users
  async getUsers() {
    try {
      const response = await this.client.get(API_ENDPOINTS.user.list);
      
      return {
        success: response.isSuccess,
        data: response.data || [],
        message: response.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch users'
      };
    }
  }

  // Get user by ID
  async getUserById(id) {
    try {
      const response = await this.client.get(API_ENDPOINTS.user.getById(id));
      
      return {
        success: response.isSuccess,
        data: response.data,
        message: response.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to fetch user'
      };
    }
  }

  // Create new user
  async createUser(userData) {
    try {
      const response = await this.client.post(API_ENDPOINTS.user.create, userData);
      
      return {
        success: response.isSuccess,
        data: response.data,
        message: response.message || (response.isSuccess ? 'User created successfully' : 'Failed to create user')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to create user'
      };
    }
  }

  // Update user
  async updateUser(id, userData) {
    try {
      const response = await this.client.put(API_ENDPOINTS.user.update(id), userData);
      
      return {
        success: response.isSuccess,
        data: response.data,
        message: response.message || (response.isSuccess ? 'User updated successfully' : 'Failed to update user')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to update user'
      };
    }
  }

  // Delete user
  async deleteUser(id) {
    try {
      const response = await this.client.delete(API_ENDPOINTS.user.delete(id));
      
      return {
        success: response.isSuccess,
        message: response.message || (response.isSuccess ? 'User deleted successfully' : 'Failed to delete user')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to delete user'
      };
    }
  }
}

export default new UserAPI();
