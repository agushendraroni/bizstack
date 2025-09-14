const express = require('express');
const cors = require('cors');
const { graphqlHTTP } = require('express-graphql');
const { buildSchema } = require('graphql');

// Load GraphQL schema
const schema = buildSchema(`
  type CompanyValidationResponse {
    isValid: Boolean!
    message: String
  }

  type Company {
    code: String!
    name: String!
    address: String
    phone: String
    email: String
    status: String!
    createdAt: String
    updatedAt: String
  }

  type Query {
    validateCompany(code: String!): CompanyValidationResponse!
    company(code: String!): Company
  }
`);

// Define resolver functions
const axios = require('axios');

const root = {
  validateCompany: async ({ code }) => {
    try {
      const response = await axios.get(`http://localhost:5003/api/v1.0/companies/validate/${code}`);
      return {
        isValid: response.data.isSuccess,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error validating company:', error);
      return {
        isValid: false,
        message: error.response?.data?.message || 'Failed to validate company'
      };
    }
  },
  company: async ({ code }) => {
    try {
      const response = await axios.get(`http://localhost:5003/api/v1.0/companies/${code}`);
      const company = response.data;
      return company;
    } catch (error) {
      console.error('Error fetching company:', error);
      return null;
    }
  }
};

// Create Express app
const app = express();

// Configure CORS
app.use(cors({
  origin: ['http://localhost:3000', 'http://localhost:8080'],
  credentials: true,
  methods: ['GET', 'POST', 'OPTIONS'],
  allowedHeaders: ['Content-Type', 'Authorization']
}));

// Health check endpoint
app.get('/health', (req, res) => {
  res.json({ 
    status: 'OK', 
    service: 'BizStack GraphQL Gateway',
    timestamp: new Date().toISOString(),
    version: '1.0.0'
  });
});

// GraphQL endpoint
app.use('/graphql', graphqlHTTP({
  schema,
  rootValue: root,
  graphiql: {
    headerEditorEnabled: true,
    shouldPersistHeaders: true
  },
  formatError: (error) => {
    console.error('GraphQL Error:', error);
    return {
      message: error.message,
      locations: error.locations,
      path: error.path
    };
  }
}));

// Start server
const PORT = process.env.PORT || 4003;
app.listen(PORT, () => {
  console.log(`🚀 BizStack GraphQL Gateway ready at http://localhost:${PORT}/graphql`);
  console.log(`📊 GraphiQL interface available at http://localhost:${PORT}/graphql`);
  console.log(`❤️  Health check at http://localhost:${PORT}/health`);
});