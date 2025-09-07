// Company Storage Utility
class CompanyStorage {
  static KEYS = {
    REMEMBERED_COMPANY: 'rememberedCompanyCode',
    REMEMBER_ENABLED: 'rememberCompanyEnabled',
    COMPANY_SETTINGS: 'companySettings'
  };

  // Valid company codes (in real app, this would come from API)
  static VALID_COMPANIES = [
    'demo',
    'test', 
    'bizstack',
    'company1',
    'company2',
    'acme',
    'startup',
    'enterprise'
  ];

  // Get remembered company code
  static getRememberedCompany() {
    return localStorage.getItem(this.KEYS.REMEMBERED_COMPANY);
  }

  // Set remembered company code
  static setRememberedCompany(companyCode) {
    if (companyCode) {
      localStorage.setItem(this.KEYS.REMEMBERED_COMPANY, companyCode.toLowerCase());
      localStorage.setItem(this.KEYS.REMEMBER_ENABLED, 'true');
    }
  }

  // Remove remembered company
  static forgetCompany() {
    localStorage.removeItem(this.KEYS.REMEMBERED_COMPANY);
    localStorage.removeItem(this.KEYS.REMEMBER_ENABLED);
    localStorage.removeItem(this.KEYS.COMPANY_SETTINGS);
  }

  // Check if remember is enabled
  static isRememberEnabled() {
    return localStorage.getItem(this.KEYS.REMEMBER_ENABLED) === 'true';
  }

  // Validate company code (async - checks with backend)
  static async isValidCompany(companyCode) {
    if (!companyCode) return false;
    
    // Fallback to static list if backend is unavailable
    const staticValid = this.VALID_COMPANIES.includes(companyCode.toLowerCase());
    
    try {
      const CompanyValidationService = (await import('../services/companyValidation.js')).default;
      return await CompanyValidationService.validateCompany(companyCode);
    } catch (error) {
      console.warn('Backend validation failed, using static list:', error);
      return staticValid;
    }
  }

  // Get company settings
  static getCompanySettings(companyCode) {
    const settings = localStorage.getItem(`${this.KEYS.COMPANY_SETTINGS}_${companyCode}`);
    return settings ? JSON.parse(settings) : {};
  }

  // Set company settings
  static setCompanySettings(companyCode, settings) {
    localStorage.setItem(
      `${this.KEYS.COMPANY_SETTINGS}_${companyCode}`, 
      JSON.stringify(settings)
    );
  }

  // Get current company from URL
  static getCurrentCompanyFromUrl() {
    const path = window.location.pathname;
    const match = path.match(/^\/([^\/]+)/);
    return match ? match[1] : null;
  }

  // Auto redirect logic
  static handleAutoRedirect() {
    const rememberedCompany = this.getRememberedCompany();
    const currentPath = window.location.pathname;
    
    // Don't redirect if already on a company path or company selector
    if (currentPath.includes('/company') || currentPath.match(/^\/[^\/]+\//)) {
      return false;
    }

    if (rememberedCompany && this.isValidCompany(rememberedCompany)) {
      window.location.href = `/${rememberedCompany}/login`;
      return true;
    }
    
    return false;
  }
}

export default CompanyStorage;