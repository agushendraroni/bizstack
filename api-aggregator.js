// Simple API Aggregator for BizStack Frontend
// Provides single endpoint for all working services

const express = require('express');
const cors = require('cors');
const axios = require('axios');

const app = express();
const PORT = 3001;

// Middleware
app.use(cors());
app.use(express.json());

// Service URLs
const services = {
  auth: 'http://localhost:5001',
  user: 'http://localhost:5002', 
  product: 'http://localhost:5004',
  transaction: 'http://localhost:5006'
};

// Health Check
app.get('/health', (req, res) => {
  res.json({ 
    status: 'OK', 
    message: 'BizStack API Aggregator',
    services: Object.keys(services)
  });
});

// Auth Service Proxy
app.use('/api/auth', async (req, res) => {
  try {
    const response = await axios({
      method: req.method,
      url: `${services.auth}/api${req.path}`,
      data: req.body,
      headers: req.headers
    });
    res.json(response.data);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// User Service Proxy
app.use('/api/users', async (req, res) => {
  try {
    const response = await axios({
      method: req.method,
      url: `${services.user}/api/Users${req.path}`,
      data: req.body,
      headers: req.headers
    });
    res.json(response.data);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Product Service Proxy
app.use('/api/products', async (req, res) => {
  try {
    const response = await axios({
      method: req.method,
      url: `${services.product}/api/Products${req.path}`,
      data: req.body,
      headers: req.headers
    });
    res.json(response.data);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Transaction Service Proxy
app.use('/api/orders', async (req, res) => {
  try {
    const response = await axios({
      method: req.method,
      url: `${services.transaction}/api/Orders${req.path}`,
      data: req.body,
      headers: req.headers
    });
    res.json(response.data);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Aggregated Dashboard Data
app.get('/api/dashboard', async (req, res) => {
  try {
    const [users, products, orders] = await Promise.all([
      axios.get(`${services.user}/api/Users`),
      axios.get(`${services.product}/api/Products`),
      axios.get(`${services.transaction}/api/Orders`)
    ]);

    res.json({
      isSuccess: true,
      data: {
        totalUsers: users.data.data?.length || 0,
        totalProducts: products.data.data?.length || 0,
        totalOrders: orders.data.data?.length || 0,
        services: {
          auth: 'online',
          user: 'online',
          product: 'online',
          transaction: 'online'
        }
      }
    });
  } catch (error) {
    res.status(500).json({ 
      isSuccess: false, 
      error: error.message 
    });
  }
});

app.listen(PORT, () => {
  console.log(`🚀 BizStack API Aggregator running on port ${PORT}`);
  console.log(`📊 Dashboard: http://localhost:${PORT}/api/dashboard`);
  console.log(`🔗 Health: http://localhost:${PORT}/health`);
});
