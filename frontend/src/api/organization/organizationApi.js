// Organization API Service
import { apiClients, API_ENDPOINTS } from '../../services/api/apiConfig';

class OrganizationAPI {
  constructor() {
    this.client = apiClients.organization;
  }

  // Get all companies
  async getCompanies() {
    try {
      const response = await this.client.get(API_ENDPOINTS.organization.companies);
      
      return {
        success: response.isSuccess,
        data: response.data || [],
        message: response.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch companies'
      };
    }
  }

  // Create new company
  async createCompany(companyData) {
    try {
      const response = await this.client.post(API_ENDPOINTS.organization.createCompany, companyData);
      
      return {
        success: response.isSuccess,
        data: response.data,
        message: response.message || (response.isSuccess ? 'Company created successfully' : 'Failed to create company')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to create company'
      };
    }
  }

  // Update company
  async updateCompany(id, companyData) {
    try {
      const response = await this.client.put(API_ENDPOINTS.organization.updateCompany(id), companyData);
      
      return {
        success: response.isSuccess,
        data: response.data,
        message: response.message || (response.isSuccess ? 'Company updated successfully' : 'Failed to update company')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to update company'
      };
    }
  }

  // Delete company
  async deleteCompany(id) {
    try {
      const response = await this.client.delete(API_ENDPOINTS.organization.deleteCompany(id));
      
      return {
        success: response.isSuccess,
        message: response.message || (response.isSuccess ? 'Company deleted successfully' : 'Failed to delete company')
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to delete company'
      };
    }
  }
}

export default new OrganizationAPI();
