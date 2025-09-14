-- Auth Service Database Insert Queries

-- 1. Insert Permissions
INSERT INTO "Permissions" ("Id", "Name", "Description", "CompanyId", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
VALUES 
    (gen_random_uuid(), 'user.read', 'Read user data', NULL, NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'user.write', 'Create/update user data', NULL, NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'user.delete', 'Delete user data', NULL, NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'admin.full', 'Full admin access', NULL, NOW(), 'system', true, false, NULL);

-- 2. Insert Roles
INSERT INTO "Roles" ("Id", "Name", "Description", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
VALUES 
    (gen_random_uuid(), 'Admin', 'System administrator', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'User', 'Regular user', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'Manager', 'Manager role', NOW(), 'system', true, false, NULL);

-- 3. Insert Users (password: "password123" hashed with BCrypt)
INSERT INTO "Users" ("Id", "Username", "PasswordHash", "LoginFailCount", "IsLocked", "UserProfileId", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
VALUES 
    (gen_random_uuid(), 'admin', '$2a$11$K2CtDP7zSGOKgjXjxD8E4.ZKz5E5D5E5D5E5D5E5D5E5D5E5D5E5D5', 0, false, NULL, NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'user1', '$2a$11$K2CtDP7zSGOKgjXjxD8E4.ZKz5E5D5E5D5E5D5E5D5E5D5E5D5E5D5', 0, false, NULL, NOW(), 'system', true, false, NULL);

-- 4. Link Roles to Permissions
INSERT INTO "RolePermissions" ("Id", "RoleId", "PermissionId", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    r."Id",
    p."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Roles" r, "Permissions" p
WHERE (r."Name" = 'Admin' AND p."Name" IN ('user.read', 'user.write', 'user.delete', 'admin.full'))
   OR (r."Name" = 'User' AND p."Name" = 'user.read')
   OR (r."Name" = 'Manager' AND p."Name" IN ('user.read', 'user.write'));

-- 5. Assign Roles to Users
INSERT INTO "UserRoles" ("Id", "UserId", "RoleId", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    u."Id",
    r."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Users" u, "Roles" r
WHERE (u."Username" = 'admin' AND r."Name" = 'Admin')
   OR (u."Username" = 'user1' AND r."Name" = 'User');