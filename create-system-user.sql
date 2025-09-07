-- Create system user for frontend-backend communication
INSERT INTO "Users" (
    "Id", 
    "Username", 
    "PasswordHash", 
    "LoginFailCount", 
    "LastLoginAt", 
    "LastFailedLoginAt", 
    "IsLocked", 
    "UserProfileId", 
    "CompanyId", 
    "CreatedAt", 
    "UpdatedAt", 
    "CreatedBy", 
    "UpdatedBy", 
    "IsActive", 
    "IsDeleted", 
    "TenantId"
)
SELECT 
    gen_random_uuid(),
    'system_frontend',
    '$2a$11$8K1p/a0dclxKANppz4eaU.hsRBcXSKXn4Z3KJa6Vs8Mw6.mIEO4W2', -- Password: SysF3nt3nd2024!@#
    0, -- LoginFailCount
    NULL, -- LastLoginAt
    NULL, -- LastFailedLoginAt
    false, -- IsLocked
    NULL, -- UserProfileId
    NULL, -- CompanyId (system user, no specific company)
    NOW(), -- CreatedAt
    NULL, -- UpdatedAt
    'SYSTEM', -- CreatedBy
    NULL, -- UpdatedBy
    true, -- IsActive
    false, -- IsDeleted
    0 -- TenantId (system tenant)
WHERE NOT EXISTS (
    SELECT 1 FROM "Users" WHERE "Username" = 'system_frontend'
);