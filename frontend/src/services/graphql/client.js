// GraphQL Client for BizStack
const GRAPHQL_ENDPOINT = process.env.REACT_APP_GRAPHQL_ENDPOINT || 'http://localhost:4000/graphql';

class GraphQLClient {
  constructor(endpoint = GRAPHQL_ENDPOINT) {
    this.endpoint = endpoint;
  }

  async query(query, variables = {}) {
    try {
      const response = await fetch(this.endpoint, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
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
}

export default new GraphQLClient();