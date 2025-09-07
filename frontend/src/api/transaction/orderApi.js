// Order API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';
import { GET_ALL_ORDERS_QUERY, CREATE_ORDER_MUTATION } from '../../services/api/queries';

class OrderAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get all orders
  async getAllOrders() {
    try {
      const response = await this.client.query(GET_ALL_ORDERS_QUERY);
      const orderData = response.TransactionService_getOrdersControllerGetAllOrders;
      
      return {
        success: orderData.isSuccess,
        data: orderData.data || [],
        message: orderData.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch orders'
      };
    }
  }

  // Create order
  async createOrder(orderData) {
    try {
      const response = await this.client.mutate(CREATE_ORDER_MUTATION, {
        createOrderDto: orderData
      });
      const result = response.TransactionService_postOrdersControllerCreateOrder;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to create order'
      };
    }
  }
}

export default new OrderAPI();