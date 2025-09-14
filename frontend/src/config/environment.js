const config = {
  apiUrl: process.env.REACT_APP_GRAPHQL_ENDPOINT,
  authUrl: process.env.REACT_APP_AUTH_SERVICE,
  n8nUrl: process.env.REACT_APP_N8N_ENDPOINT,
  environment: process.env.REACT_APP_ENVIRONMENT || 'development',
  isDevelopment: process.env.REACT_APP_ENVIRONMENT === 'development',
  isProduction: process.env.REACT_APP_ENVIRONMENT === 'production'
};

export default config;