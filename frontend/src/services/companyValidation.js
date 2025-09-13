// Company Validation Service via GraphQL
class CompanyValidationService {
  static SYSTEM_CREDENTIALS = {
    username: 'system_frontend',
    password: 'SysF3nt3nd2024!@#'
  };

  static systemToken = null;

  static async getSystemToken() {
    if (this.systemToken) return this.systemToken;

    try {
      const response = await fetch('http://localhost:5001/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(this.SYSTEM_CREDENTIALS)
      });

      const result = await response.json();
      if (result.isSuccess && result.data && result.data.accessToken) {
        this.systemToken = result.data.accessToken;
        return this.systemToken;
      }
      throw new Error('System auth failed');
    } catch (error) {
      console.error('System auth error:', error);
      return null;
    }
  }

  static async validateCompany(companyCode) {
    // Fallback validation for demo
    const validCompanies = ['demo', 'test', 'bizstack', 'company1', 'company2'];
    return validCompanies.includes(companyCode.toLowerCase());
  }
}

export default CompanyValidationService;