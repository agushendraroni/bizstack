-- User Service Database Insert Queries

-- 1. Insert Users
INSERT INTO "Users" ("Id", "Username", "Email", "FirstName", "LastName", "PhoneNumber", "OrganizationId", "Position", "Department", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
VALUES 
    (gen_random_uuid(), 'admin', 'admin@bizstack.com', 'Admin', 'System', '+6281234567890', NULL, 'System Administrator', 'IT', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'john.doe', 'john.doe@company.com', 'John', 'Doe', '+6281234567891', NULL, 'Manager', 'Sales', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'jane.smith', 'jane.smith@company.com', 'Jane', 'Smith', '+6281234567892', NULL, 'Staff', 'Marketing', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'mike.wilson', 'mike.wilson@company.com', 'Mike', 'Wilson', '+6281234567893', NULL, 'Developer', 'IT', NOW(), 'system', true, false, NULL);

-- 2. Insert User Profiles (minimal columns)
INSERT INTO "UserProfiles" ("Id", "UserId", "FullName", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    u."Id",
    'Admin System',
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'admin'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'John Doe',
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'john.doe'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'Jane Smith',
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'jane.smith'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'Mike Wilson',
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'mike.wilson';

-- 3. Insert User Preferences
INSERT INTO "UserPreferences" ("Id", "UserId", "Language", "Theme", "Timezone", "ReceiveNotifications", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    u."Id",
    'en',
    'dark',
    'Asia/Jakarta',
    true,
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'admin'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'en',
    'light',
    'Asia/Jakarta',
    true,
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'john.doe'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'id',
    'light',
    'Asia/Jakarta',
    false,
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'jane.smith'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'en',
    'dark',
    'Asia/Jakarta',
    true,
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'mike.wilson';

-- 4. Insert User Activity Logs
INSERT INTO "UserActivityLogs" ("Id", "UserId", "Activity", "Description", "IpAddress", "UserAgent", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    u."Id",
    'Login',
    'User logged in successfully',
    '192.168.1.100',
    'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36',
    NOW() - INTERVAL '1 hour',
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'admin'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'UpdateProfile',
    'User updated profile information',
    '192.168.1.101',
    'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36',
    NOW() - INTERVAL '2 hours',
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'john.doe'
UNION ALL
SELECT 
    gen_random_uuid(),
    u."Id",
    'Login',
    'User logged in from mobile device',
    '192.168.1.102',
    'Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15',
    NOW() - INTERVAL '30 minutes',
    'system',
    true,
    false,
    NULL
FROM "Users" u WHERE u."Username" = 'jane.smith';