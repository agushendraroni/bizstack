// Customer API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';
import { GET_ALL_CUSTOMERS_QUERY, CREATE_CUSTOMER_MUTATION } from '../../services/api/queries';

class CustomerAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get all customers
  async getAllCustomers() {
    try {
      const response = await this.client.query(GET_ALL_CUSTOMERS_QUERY);
      const customerData = response.CustomerService_getCustomersControllerGetAllCustomers;
      
      return {
        success: customerData.isSuccess,
        data: customerData.data || [],
        message: customerData.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch customers'
      };
    }
  }

  // Create customer
  async createCustomer(customerData) {
    try {
      const response = await this.client.mutate(CREATE_CUSTOMER_MUTATION, {
        createCustomerDto: customerData
      });
      const result = response.CustomerService_postCustomersControllerCreateCustomer;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to create customer'
      };
    }
  }
}

export default new CustomerAPI();