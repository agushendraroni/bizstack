// BizStack Core Business Aggregates
// Essential GraphQL resolvers for any business

const businessAggregates = {
  Query: {
    // 1. DASHBOARD - Most important for any business
    async businessDashboard(root, args, context, info) {
      const { fetch } = context;
      
      try {
        // Parallel fetch all dashboard data
        const [salesData, customerData, productData, orderData] = await Promise.all([
          fetch('http://report-service:5007/api/reports/dashboard'),
          fetch('http://customer-service:5005/api/customers'),
          fetch('http://product-service:5004/api/products/low-stock'),
          fetch('http://transaction-service:5006/api/orders?status=pending')
        ]);

        return {
          todaySales: salesData.data?.todaySales || 0,
          totalCustomers: customerData.data?.length || 0,
          lowStockCount: productData.data?.length || 0,
          pendingOrders: orderData.data?.length || 0,
          salesTrend: salesData.data?.weeklySales || [],
          topProducts: salesData.data?.topProducts || [],
          recentCustomers: customerData.data?.slice(0, 5) || [],
          alerts: {
            lowStock: productData.data?.length > 0,
            pendingOrders: orderData.data?.length > 0,
            newCustomers: customerData.data?.filter(c => 
              new Date(c.createdAt) > new Date(Date.now() - 24*60*60*1000)
            ).length
          }
        };
      } catch (error) {
        throw new Error(`Dashboard fetch failed: ${error.message}`);
      }
    },

    // 2. BUSINESS OVERVIEW - Complete business state
    async businessOverview(root, args, context, info) {
      const { fetch } = context;
      
      try {
        const [company, users, products, customers, orders] = await Promise.all([
          fetch('http://organization-service:5003/api/companies'),
          fetch('http://user-service:5002/api/users'),
          fetch('http://product-service:5004/api/products'),
          fetch('http://customer-service:5005/api/customers'),
          fetch('http://transaction-service:5006/api/orders')
        ]);

        return {
          company: company.data?.[0] || null,
          stats: {
            totalUsers: users.data?.length || 0,
            totalProducts: products.data?.length || 0,
            totalCustomers: customers.data?.length || 0,
            totalOrders: orders.data?.length || 0,
            activeUsers: users.data?.filter(u => u.isActive).length || 0,
            vipCustomers: customers.data?.filter(c => c.customerType === 'VIP').length || 0
          },
          health: {
            services: ['auth', 'user', 'product', 'customer', 'transaction', 'notification'],
            status: 'healthy',
            lastUpdated: new Date().toISOString()
          }
        };
      } catch (error) {
        throw new Error(`Business overview fetch failed: ${error.message}`);
      }
    },

    // 3. SALES ANALYTICS - Critical for revenue tracking
    async salesAnalytics(root, { startDate, endDate }, context, info) {
      const { fetch } = context;
      
      try {
        const [salesReport, topProducts, customerStats] = await Promise.all([
          fetch(`http://report-service:5007/api/reports/sales?startDate=${startDate}&endDate=${endDate}`),
          fetch('http://report-service:5007/api/reports/products'),
          fetch('http://report-service:5007/api/reports/customers')
        ]);

        return {
          period: { startDate, endDate },
          revenue: {
            total: salesReport.data?.reduce((sum, day) => sum + day.totalSales, 0) || 0,
            average: salesReport.data?.length ? 
              salesReport.data.reduce((sum, day) => sum + day.totalSales, 0) / salesReport.data.length : 0,
            trend: salesReport.data || []
          },
          orders: {
            total: salesReport.data?.reduce((sum, day) => sum + day.totalOrders, 0) || 0,
            average: salesReport.data?.length ?
              salesReport.data.reduce((sum, day) => sum + day.totalOrders, 0) / salesReport.data.length : 0
          },
          products: {
            topSelling: topProducts.data?.slice(0, 10) || [],
            lowPerforming: topProducts.data?.slice(-5) || []
          },
          customers: {
            topSpenders: customerStats.data?.slice(0, 10) || [],
            newCustomers: customerStats.data?.filter(c => 
              new Date(c.lastOrderDate) >= new Date(startDate)
            ).length || 0
          }
        };
      } catch (error) {
        throw new Error(`Sales analytics fetch failed: ${error.message}`);
      }
    },

    // 4. INVENTORY STATUS - Essential for stock management
    async inventoryStatus(root, args, context, info) {
      const { fetch } = context;
      
      try {
        const [allProducts, lowStock, categories] = await Promise.all([
          fetch('http://product-service:5004/api/products'),
          fetch('http://product-service:5004/api/products/low-stock'),
          fetch('http://product-service:5004/api/categories')
        ]);

        const products = allProducts.data || [];
        
        return {
          totalProducts: products.length,
          totalValue: products.reduce((sum, p) => sum + (p.price * p.stock), 0),
          lowStockItems: lowStock.data || [],
          outOfStock: products.filter(p => p.stock === 0),
          categories: categories.data?.map(cat => ({
            ...cat,
            productCount: products.filter(p => p.categoryId === cat.id).length,
            totalValue: products
              .filter(p => p.categoryId === cat.id)
              .reduce((sum, p) => sum + (p.price * p.stock), 0)
          })) || [],
          alerts: {
            criticalStock: lowStock.data?.filter(p => p.stock <= 5).length || 0,
            reorderNeeded: lowStock.data?.length || 0,
            overstocked: products.filter(p => p.stock > 100).length
          }
        };
      } catch (error) {
        throw new Error(`Inventory status fetch failed: ${error.message}`);
      }
    }
  },

  Mutation: {
    // 1. QUICK SALE - Most common business operation
    async processQuickSale(root, { customerId, items, paymentMethod }, context, info) {
      const { fetch } = context;
      
      try {
        // Create order
        const orderData = {
          customerId,
          items,
          paymentMethod,
          status: 'completed',
          total: items.reduce((sum, item) => sum + (item.price * item.quantity), 0)
        };
        
        const order = await fetch('http://transaction-service:5006/api/orders', {
          method: 'POST',
          body: JSON.stringify(orderData)
        });

        // Update inventory for each item
        await Promise.all(items.map(item => 
          fetch(`http://product-service:5004/api/products/${item.productId}/stock`, {
            method: 'PATCH',
            body: JSON.stringify({ quantity: -item.quantity })
          })
        ));

        // Update customer stats
        await fetch(`http://customer-service:5005/api/customers/${customerId}/stats`, {
          method: 'PATCH',
          body: JSON.stringify({ orderAmount: orderData.total })
        });

        // Send receipt notification
        await fetch('http://notification-service:5008/api/notifications/email', {
          method: 'POST',
          body: JSON.stringify({
            type: 'Email',
            recipient: 'customer@email.com',
            subject: 'Purchase Receipt',
            message: `Thank you for your purchase. Order #${order.data.id}`
          })
        });

        return {
          success: true,
          order: order.data,
          message: 'Sale processed successfully'
        };
      } catch (error) {
        return {
          success: false,
          order: null,
          message: `Sale processing failed: ${error.message}`
        };
      }
    },

    // 2. CUSTOMER ONBOARDING - Essential for growth
    async onboardCustomer(root, { customerData, sendWelcome }, context, info) {
      const { fetch } = context;
      
      try {
        // Create customer
        const customer = await fetch('http://customer-service:5005/api/customers', {
          method: 'POST',
          body: JSON.stringify(customerData)
        });

        // Create user account if email provided
        let user = null;
        if (customerData.email) {
          user = await fetch('http://user-service:5002/api/users', {
            method: 'POST',
            body: JSON.stringify({
              email: customerData.email,
              name: customerData.name,
              role: 'customer'
            })
          });
        }

        // Send welcome notification
        if (sendWelcome && customerData.email) {
          await fetch('http://notification-service:5008/api/notifications/email', {
            method: 'POST',
            body: JSON.stringify({
              type: 'Email',
              recipient: customerData.email,
              subject: 'Welcome to Our Business!',
              message: `Welcome ${customerData.name}! Thank you for joining us.`
            })
          });
        }

        return {
          success: true,
          customer: customer.data,
          user: user?.data,
          message: 'Customer onboarded successfully'
        };
      } catch (error) {
        return {
          success: false,
          customer: null,
          user: null,
          message: `Customer onboarding failed: ${error.message}`
        };
      }
    },

    // 3. INVENTORY UPDATE - Critical for stock management
    async updateInventory(root, { updates }, context, info) {
      const { fetch } = context;
      
      try {
        const results = await Promise.all(
          updates.map(async (update) => {
            try {
              const result = await fetch(`http://product-service:5004/api/products/${update.productId}/stock`, {
                method: 'PATCH',
                body: JSON.stringify({ quantity: update.quantity })
              });
              return { productId: update.productId, success: true, data: result.data };
            } catch (error) {
              return { productId: update.productId, success: false, error: error.message };
            }
          })
        );

        const successful = results.filter(r => r.success);
        const failed = results.filter(r => !r.success);

        return {
          success: failed.length === 0,
          updated: successful.length,
          failed: failed.length,
          results,
          message: `Updated ${successful.length} products, ${failed.length} failed`
        };
      } catch (error) {
        return {
          success: false,
          updated: 0,
          failed: updates.length,
          results: [],
          message: `Inventory update failed: ${error.message}`
        };
      }
    }
  }
};

module.exports = businessAggregates;