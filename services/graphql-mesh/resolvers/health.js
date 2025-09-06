module.exports = {
  Query: {
    health: () => ({
      status: 'OK',
      timestamp: new Date().toISOString(),
      services: {
        auth: 'http://auth-service:5001/health',
        user: 'http://user-service:5002/health',
        organization: 'http://organization-service:5003/health',
        product: 'http://product-service:5004/health',
        customer: 'http://customer-service:5005/health',
        transaction: 'http://transaction-service:5006/health',
        report: 'http://report-service:5007/health',
        notification: 'http://notification-service:5008/health',
        fileStorage: 'http://file-storage-service:5009/health'
      }
    })
  }
};
