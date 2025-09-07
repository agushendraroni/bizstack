// Role API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';

class RoleAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get all roles
  async getRoles() {
    try {
      // For now, return static roles - can be extended with GraphQL query later
      return {
        success: true,
        data: [
          { id: 1, name: 'Admin', description: 'Full system access' },
          { id: 2, name: 'Manager', description: 'Management access' },
          { id: 3, name: 'User', description: 'Basic user access' },
          { id: 4, name: 'Viewer', description: 'Read-only access' }
        ],
        message: 'Roles retrieved successfully'
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch roles'
      };
    }
  }
}

export default new RoleAPI();