// Default Company Data - PT Blitz Technology Indonesia
export const defaultCompany = {
  id: "blitz-001",
  code: "BLITZ",
  name: "PT Blitz Technology Indonesia",
  description: "Leading business technology solutions provider in Indonesia. Specializing in enterprise software development, digital transformation, and business process automation. We help businesses accelerate their digital journey with innovative technology solutions.",
  
  // Contact Information
  address: "Jl. HR Rasuna Said Kav. C-5, Kuningan, Jakarta Selatan 12940, Indonesia",
  phone: "+62-21-5794-8000",
  email: "info@blitz.co.id",
  website: "https://www.blitz.co.id",
  
  // Legal Documents
  npwp: "01.234.567.8-901.000",
  nib: "1234567890123456",
  siup: "SIUP/BLITZ/2023/001",
  
  // Status
  isActive: true,
  createdAt: "2023-01-01T00:00:00Z",
  updatedAt: "2024-01-01T00:00:00Z",
  
  // Multi-tenant Info
  tenantUrl: "BLITZ/login",
  maxUsers: 1000,
  currentUsers: 1,
  
  // Default Admin User
  adminUser: {
    id: "admin-blitz-001",
    firstName: "System",
    lastName: "Administrator",
    username: "admin",
    email: "admin@blitz.co.id",
    phoneNumber: "+62-21-5794-8001",
    position: "System Administrator",
    department: "Information Technology",
    isActive: true,
    isAdmin: true,
    createdAt: "2023-01-01T00:00:00Z",
    
    // Default Password (should be changed on first login)
    defaultPassword: "admin123",
    
    // Permissions
    permissions: [
      "users_read", "users_write", "users_delete",
      "roles_read", "roles_write", "roles_delete",
      "organizations_read", "organizations_write", "organizations_delete",
      "products_read", "products_write", "products_delete",
      "customers_read", "customers_write", "customers_delete",
      "transactions_read", "transactions_write", "transactions_delete",
      "reports_read", "reports_write", "reports_delete",
      "notifications_read", "notifications_write", "notifications_delete",
      "files_read", "files_write", "files_delete"
    ]
  },
  
  // Company Settings
  settings: {
    timezone: "Asia/Jakarta",
    currency: "IDR",
    language: "id",
    dateFormat: "DD/MM/YYYY",
    theme: "light",
    
    // Business Settings
    businessType: "Technology Services",
    industry: "Information Technology",
    employeeCount: "1-50",
    
    // Feature Flags
    features: {
      multiTenant: true,
      userManagement: true,
      roleManagement: true,
      organizationManagement: true,
      menuTreeManagement: true,
      companyManagement: true,
      profileManagement: true
    }
  },
  
  // Branding
  branding: {
    primaryColor: "#007bff",
    secondaryColor: "#6c757d",
    logoUrl: "/images/blitz-logo.png",
    faviconUrl: "/images/blitz-favicon.ico",
    companySlogan: "Accelerating Digital Transformation"
  }
};

// Default Login Credentials
export const defaultCredentials = {
  companyCode: "BLITZ",
  username: "admin",
  password: "admin123",
  loginUrl: "BLITZ/login"
};

// Export for easy access
export default {
  company: defaultCompany,
  credentials: defaultCredentials
};
