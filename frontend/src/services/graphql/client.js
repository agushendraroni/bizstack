// GraphQL Client for BizStack
import API_CONFIG from '../../config/apiConfig';

const GRAPHQL_ENDPOINT = API_CONFIG.GRAPHQL_ENDPOINT;

class GraphQLClient {
  constructor(endpoint = GRAPHQL_ENDPOINT) {
    this.endpoint = endpoint;
  }

  async query(query, variables = {}) {
    try {
      const token = localStorage.getItem('accessToken');
      const headers = {
        'Content-Type': 'application/json',
      };
      
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
      
      const response = await fetch(this.endpoint, {
        method: 'POST',
        headers,
        body: JSON.stringify({
          query,
          variables
        })
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const result = await response.json();
      
      if (result.errors) {
        throw new Error(result.errors[0].message);
      }

      return result.data;
    } catch (error) {
      console.error('GraphQL query failed:', error);
      throw error;
    }
  }

  // Company queries
  async getCompanyByCode(code) {
    const query = `
      query GetCompanyByCode($code: String!) {
        api_Companies_code_by_code(code: $code) {
          isSuccess
          message
          data {
            tenantId
            name
            code
            description
            address
            phone
            email
            website
            isActive
            createdAt
          }
        }
      }
    `;
    
    const result = await this.query(query, { code });
    return result.api_Companies_code_by_code;
  }

  async getAllCompanies() {
    const query = `
      query GetAllCompanies {
        api_Companies {
          isSuccess
          message
          data {
            tenantId
            name
            code
            description
            address
            phone
            email
            website
            isActive
            createdAt
          }
        }
      }
    `;
    
    const result = await this.query(query);
    return result.api_Companies;
  }

  async getPublicCompanies() {
    const query = `
      query GetPublicCompanies {
        api_Companies_public_list {
          isSuccess
          message
          data {
            tenantId
            name
            code
            description
            address
            phone
            email
            website
            isActive
            createdAt
          }
        }
      }
    `;
    
    // Don't send auth header for public endpoint
    const response = await fetch(this.endpoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        query
      })
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const result = await response.json();
    
    if (result.errors) {
      throw new Error(result.errors[0].message);
    }

    return result.data.api_Companies_public_list;
  }
}

export default new GraphQLClient();