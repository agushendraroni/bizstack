// Dashboard API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';
import { BUSINESS_DASHBOARD_QUERY } from '../../services/api/queries';

class DashboardAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get business dashboard data
  async getDashboardData() {
    try {
      const response = await this.client.query(BUSINESS_DASHBOARD_QUERY);
      const dashboardData = response.ReportService_getReportsControllerGetDashboard;
      
      return {
        success: dashboardData.isSuccess,
        data: dashboardData.data || {
          todaySales: 0,
          todayOrders: 0,
          totalCustomers: 0,
          lowStockProducts: 0
        },
        message: dashboardData.message
      };
    } catch (error) {
      return {
        success: false,
        data: {
          todaySales: 0,
          todayOrders: 0,
          totalCustomers: 0,
          lowStockProducts: 0
        },
        message: error.message || 'Failed to fetch dashboard data'
      };
    }
  }
}

export default new DashboardAPI();