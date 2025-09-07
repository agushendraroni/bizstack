// User API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';
import { GET_ALL_USERS_QUERY } from '../../services/api/queries';

class UserAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get all users with pagination
  async getUsers(page = 1, pageSize = 10, search = '') {
    try {
      const response = await this.client.query(GET_ALL_USERS_QUERY);
      const userData = response.UserService_getUserProfilesControllerGetProfiles;
      
      if (userData.isSuccess) {
        let users = userData.data || [];
        
        // Apply search filter if provided
        if (search) {
          users = users.filter(user => 
            (user.fullName && user.fullName.toLowerCase().includes(search.toLowerCase())) ||
            (user.email && user.email.toLowerCase().includes(search.toLowerCase()))
          );
        }
        
        // Apply pagination
        const startIndex = (page - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        const paginatedUsers = users.slice(startIndex, endIndex);
        
        return {
          success: true,
          data: paginatedUsers,
          pagination: {
            page,
            pageSize,
            total: users.length,
            totalPages: Math.ceil(users.length / pageSize)
          },
          message: userData.message
        };
      }
      
      return {
        success: false,
        data: [],
        message: userData.message || 'Failed to fetch users'
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
      const response = await this.client.query(GET_ALL_USERS_QUERY);
      const userData = response.UserService_getUserProfilesControllerGetProfiles;
      
      if (userData.isSuccess) {
        const user = userData.data && userData.data.find(u => u.id === id || u.userId === id);
        
        if (user) {
          return {
            success: true,
            data: user,
            message: 'User retrieved successfully'
          };
        }
        
        return {
          success: false,
          data: null,
          message: 'User not found'
        };
      }
      
      return {
        success: false,
        data: null,
        message: userData.message || 'Failed to fetch user'
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to fetch user'
      };
    }
  }

  // Create user (placeholder)
  async createUser(userData) {
    return {
      success: false,
      message: 'User creation not implemented yet'
    };
  }

  // Update user (placeholder)
  async updateUser(id, userData) {
    return {
      success: false,
      message: 'User update not implemented yet'
    };
  }

  // Delete user (placeholder)
  async deleteUser(id) {
    return {
      success: false,
      message: 'User deletion not implemented yet'
    };
  }
}

export default new UserAPI();