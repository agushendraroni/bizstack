# 🔐 System Credentials - BizStack

## Frontend System User

**Username:** `system_frontend`  
**Password:** `SysF3nt3nd2024!@#`

### Purpose
- Frontend authentication to backend services
- Company validation via GraphQL
- System-level operations without user login

### Usage
- Used in `CompanyValidationService` 
- Authenticates with Auth Service to get JWT token
- Token used for GraphQL queries to Organization Service

### Security Notes
- System user has no specific company assignment
- Limited to validation and system operations only
- Token expires in 1 hour, auto-refreshed as needed