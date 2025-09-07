const config = {
  apiUrl: process.env.REACT_APP_API_URL || 'http://localhost:4000/graphql',
  authUrl: process.env.REACT_APP_AUTH_URL || 'http://localhost:5001',
  n8nUrl: process.env.REACT_APP_N8N_URL || 'http://localhost:5678',
  environment: process.env.REACT_APP_ENVIRONMENT || 'development',
  isDevelopment: process.env.REACT_APP_ENVIRONMENT === 'development',
  isProduction: process.env.REACT_APP_ENVIRONMENT === 'production'
};

export default config;