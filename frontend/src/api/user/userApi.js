// User API Service with Pagination
import { apiClients, API_ENDPOINTS } from '../../services/api/apiConfig';

class UserAPI {
  constructor() {
    this.client = apiClients.user;
  }

  // Get users with pagination
  async getUsers(page = 1, pageSize = 10, search = '') {
    try {
      let endpoint = `${API_ENDPOINTS.user.list}?page=${page}&pageSize=${pageSize}`;
      if (search) {
        endpoint += `&search=${encodeURIComponent(search)}`;
      }
      
      const response = await this.client.get(endpoint);
      
      return {
        success: response.isSuccess || true,
        data: response.data || [],
        pagination: {
          currentPage: page,
          pageSize: pageSize,
          totalItems: response.totalCount || response.data?.length || 0,
          totalPages: Math.ceil((response.totalCount || response.data?.length || 0) / pageSize)
        },
        message: response.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        pagination: {
          currentPage: 1,
          pageSize: pageSize,
          totalItems: 0,
          totalPages: 0
        },
        message: error.message || 'Failed to fetch users'
      };
    }
  }

  // Get user by ID
  async getUserById(id) {
    try {
      const response = await this.client.get(API_ENDPOINTS.user.getById(id));
      
      return {
        success: response.isSuccess || true,
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
        success: response.isSuccess || true,
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
        success: response.isSuccess || true,
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
        success: response.isSuccess || true,
        message: response.message || (response.isSuccess ? 'User deleted successfully' : 'Failed to delete user')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to delete user'
      };
    }
  }

  // Search users
  async searchUsers(query, page = 1, pageSize = 10) {
    return this.getUsers(page, pageSize, query);
  }
}

export default new UserAPI();
