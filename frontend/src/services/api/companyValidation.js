const CompanyValidationService = {
  async getCompanyDetails(companyCode) {
    if (!companyCode) {
      return { success: false, error: 'Company code is required' };
    }

    try {
      const response = await fetch(`http://localhost:5003/api/v1.0/companies/code/${companyCode}`);
      
      if (!response.ok) {
        return { success: false, error: 'Company not found' };
      }
      
      const data = await response.json();
      
      if (!data.isSuccess || !data.data) {
        return { success: false, error: 'Company not found' };
      }
      
      return { success: true, data: data.data };
    } catch (error) {
      console.error('Error fetching company details:', error);
      return { success: false, error: 'Network error' };
    }
  }
};

export default CompanyValidationService;