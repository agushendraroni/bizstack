-- Customer Service Database Insert Queries

-- 1. Insert Customer Groups
INSERT INTO "CustomerGroups" ("Id", "Name", "Description", "DiscountPercentage", "MinimumSpent", "Color", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
VALUES 
    (gen_random_uuid(), 'VIP', 'VIP customers with premium benefits', 15.00, 1000000, '#FFD700', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'Regular', 'Regular customers', 5.00, 0, '#87CEEB', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'Wholesale', 'Wholesale customers', 20.00, 5000000, '#32CD32', NOW(), 'system', true, false, NULL),
    (gen_random_uuid(), 'New Customer', 'New customers', 10.00, 0, '#FFA500', NOW(), 'system', true, false, NULL);

-- 2. Insert Customers
INSERT INTO "Customers" ("Id", "Name", "Email", "Phone", "Address", "City", "PostalCode", "DateOfBirth", "Gender", "CustomerType", "TotalSpent", "TotalOrders", "LastOrderDate", "Notes", "CustomerGroupId", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    'John Doe',
    'john.doe@email.com',
    '+6281234567890',
    'Jl. Sudirman No. 123',
    'Jakarta',
    '12345',
    '1985-05-15'::date,
    'Male',
    'VIP',
    2500000.00,
    15,
    '2024-01-15'::date,
    'Loyal customer, prefers premium products',
    cg."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "CustomerGroups" cg WHERE cg."Name" = 'VIP'
UNION ALL
SELECT 
    gen_random_uuid(),
    'Jane Smith',
    'jane.smith@email.com',
    '+6281234567891',
    'Jl. Thamrin No. 456',
    'Jakarta',
    '12346',
    '1990-08-22'::date,
    'Female',
    'Regular',
    750000.00,
    8,
    '2024-01-10'::date,
    'Regular customer, likes discounts',
    cg."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "CustomerGroups" cg WHERE cg."Name" = 'Regular'
UNION ALL
SELECT 
    gen_random_uuid(),
    'PT Maju Jaya',
    'purchasing@majujaya.com',
    '+6281234567892',
    'Jl. Gatot Subroto No. 789',
    'Jakarta',
    '12347',
    NULL,
    NULL,
    'Wholesale',
    15000000.00,
    25,
    '2024-01-20'::date,
    'Wholesale partner, bulk orders',
    cg."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "CustomerGroups" cg WHERE cg."Name" = 'Wholesale'
UNION ALL
SELECT 
    gen_random_uuid(),
    'Ahmad Rizki',
    'ahmad.rizki@email.com',
    '+6281234567893',
    'Jl. Kebon Jeruk No. 321',
    'Bandung',
    '40123',
    '1988-12-03'::date,
    'Male',
    'Regular',
    0.00,
    0,
    NULL,
    'New customer from referral program',
    cg."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "CustomerGroups" cg WHERE cg."Name" = 'New Customer';

-- 3. Insert Customer Contacts
INSERT INTO "CustomerContacts" ("Id", "ContactType", "ContactValue", "Label", "IsPrimary", "Notes", "CustomerId", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "TenantId")
SELECT 
    gen_random_uuid(),
    'Phone',
    '+6281234567890',
    'Mobile',
    true,
    'Primary contact number',
    c."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Customers" c WHERE c."Name" = 'John Doe'
UNION ALL
SELECT 
    gen_random_uuid(),
    'Email',
    'john.doe@email.com',
    'Personal',
    true,
    'Primary email address',
    c."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Customers" c WHERE c."Name" = 'John Doe'
UNION ALL
SELECT 
    gen_random_uuid(),
    'WhatsApp',
    '+6281234567890',
    'WhatsApp',
    false,
    'WhatsApp contact for promotions',
    c."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Customers" c WHERE c."Name" = 'John Doe'
UNION ALL
SELECT 
    gen_random_uuid(),
    'Phone',
    '+6281234567891',
    'Mobile',
    true,
    'Primary contact number',
    c."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Customers" c WHERE c."Name" = 'Jane Smith'
UNION ALL
SELECT 
    gen_random_uuid(),
    'Email',
    'purchasing@majujaya.com',
    'Work',
    true,
    'Business email for orders',
    c."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Customers" c WHERE c."Name" = 'PT Maju Jaya'
UNION ALL
SELECT 
    gen_random_uuid(),
    'Phone',
    '+6281234567892',
    'Office',
    true,
    'Office phone number',
    c."Id",
    NOW(),
    'system',
    true,
    false,
    NULL
FROM "Customers" c WHERE c."Name" = 'PT Maju Jaya';