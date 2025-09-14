// Centralized API Configuration using Environment Variables
const API_CONFIG = {
  // Base URLs
  BASE_URL: process.env.REACT_APP_BASE_URL,
  GRAPHQL_ENDPOINT: process.env.REACT_APP_GRAPHQL_ENDPOINT,
  
  // Service endpoints
  SERVICES: {
    auth: process.env.REACT_APP_AUTH_SERVICE,
    user: process.env.REACT_APP_USER_SERVICE,
    organization: process.env.REACT_APP_ORGANIZATION_SERVICE,
    product: process.env.REACT_APP_PRODUCT_SERVICE,
    customer: process.env.REACT_APP_CUSTOMER_SERVICE,
    transaction: process.env.REACT_APP_TRANSACTION_SERVICE,
    report: process.env.REACT_APP_REPORT_SERVICE,
    notification: process.env.REACT_APP_NOTIFICATION_SERVICE,
    fileStorage: process.env.REACT_APP_FILE_STORAGE_SERVICE,
    settings: process.env.REACT_APP_SETTINGS_SERVICE
  },
  
  // External services
  N8N_ENDPOINT: process.env.REACT_APP_N8N_ENDPOINT,
  
  // API version
  API_VERSION: process.env.REACT_APP_API_VERSION,
  
  // Helper methods
  getServiceUrl: (service, path = '') => {
    const baseUrl = API_CONFIG.SERVICES[service];
    if (!baseUrl) {
      throw new Error(`Unknown service: ${service}`);
    }
    return `${baseUrl}/api/${API_CONFIG.API_VERSION}${path}`;
  },
  
  getN8nWebhook: (webhook) => {
    return `${API_CONFIG.N8N_ENDPOINT}/webhook/${webhook}`;
  }
};

export default API_CONFIG;