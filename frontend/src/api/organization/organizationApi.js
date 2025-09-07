// Organization API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';
import {
  GET_ALL_COMPANIES_QUERY,
  GET_COMPANY_BY_CODE_QUERY,
  CREATE_COMPANY_MUTATION
} from '../../services/api/queries';

class OrganizationAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get all companies
  async getCompanies() {
    try {
      const response = await this.client.query(GET_ALL_COMPANIES_QUERY);
      const companyData = response.OrganizationService_getCompaniesControllerGetAllCompanies;
      
      return {
        success: companyData.isSuccess,
        data: companyData.data || [],
        message: companyData.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch companies'
      };
    }
  }

  // Get company by code
  async getCompanyByCode(code) {
    try {
      const response = await this.client.query(GET_COMPANY_BY_CODE_QUERY, { code });
      const companyData = response.OrganizationService_getCompaniesControllerGetCompanyByCode;
      
      return {
        success: companyData.isSuccess,
        data: companyData.data,
        message: companyData.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to fetch company'
      };
    }
  }

  // Create new company
  async createCompany(companyData) {
    try {
      const response = await this.client.mutate(CREATE_COMPANY_MUTATION, {
        createCompanyDto: companyData
      });
      const result = response.OrganizationService_postCompaniesControllerCreateCompany;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        message: error.message || 'Failed to create company'
      };
    }
  }
}

export default new OrganizationAPI();