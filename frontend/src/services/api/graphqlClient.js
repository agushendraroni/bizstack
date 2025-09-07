// GraphQL Client for BizStack
class GraphQLClient {
  constructor() {
    this.endpoint = process.env.REACT_APP_GRAPHQL_ENDPOINT || 'http://localhost:4000/graphql';
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

  async query(query, variables = {}) {
    try {
      const response = await fetch(this.endpoint, {
        method: 'POST',
        headers: this.getHeaders(),
        body: JSON.stringify({
          query,
          variables
        })
      });

      const result = await response.json();
      
      if (result.errors) {
        console.error('GraphQL errors:', result.errors);
        throw new Error(result.errors[0].message);
      }

      return result.data;
    } catch (error) {
      console.error('GraphQL request failed:', error);
      throw error;
    }
  }

  async mutate(mutation, variables = {}) {
    return this.query(mutation, variables);
  }
}

export const graphqlClient = new GraphQLClient();
export default GraphQLClient;