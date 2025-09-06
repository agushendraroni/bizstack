// Role API Service
import { apiClients, API_ENDPOINTS } from '../../services/api/apiConfig';

class RoleAPI {
  constructor() {
    this.client = apiClients.auth;
  }

  // Get all roles with permissions
  async getRoles() {
    try {
      const response = await this.client.get(API_ENDPOINTS.auth.roles);
      
      return {
        success: response.isSuccess || true,
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

  // Create new role
  async createRole(roleData) {
    try {
      const response = await this.client.post('/api/Roles', roleData);
      
      return {
        success: response.isSuccess || true,
        data: response.data,
        message: response.message || 'Role created successfully'
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to create role'
      };
    }
  }

  // Update role
  async updateRole(id, roleData) {
    try {
      const response = await this.client.put(`/api/Roles/${id}`, roleData);
      
      return {
        success: response.isSuccess || true,
        data: response.data,
        message: response.message || 'Role updated successfully'
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to update role'
      };
    }
  }

  // Delete role
  async deleteRole(id) {
    try {
      const response = await this.client.delete(`/api/Roles/${id}`);
      
      return {
        success: response.isSuccess || true,
        message: response.message || 'Role deleted successfully'
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to delete role'
      };
    }
  }

  // Get permissions
  async getPermissions() {
    try {
      const response = await this.client.get('/api/Permissions');
      
      return {
        success: response.isSuccess || true,
        data: response.data || [],
        message: response.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch permissions'
      };
    }
  }

  // Assign permissions to role
  async assignPermissions(roleId, permissionIds) {
    try {
      const response = await this.client.post(`/api/Roles/${roleId}/permissions`, {
        permissionIds: permissionIds
      });
      
      return {
        success: response.isSuccess || true,
        message: response.message || 'Permissions assigned successfully'
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to assign permissions'
      };
    }
  }
}

export default new RoleAPI();
