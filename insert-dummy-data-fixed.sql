-- BizStack Dummy Data Insert Script - Fixed for TenantId
-- Company TenantId is auto-generated, other entities use it as foreign key

-- =============================================
-- 1. ORGANIZATION SERVICE - Companies (TenantId source)
-- =============================================

-- Companies (TenantId auto-generated)
INSERT INTO organization_db."Companies" ("Code", "Name", "Description", "Address", "Phone", "Email", "Website", "CreatedAt", "IsActive", "IsDeleted") VALUES
('BLITZ', 'PT Blitz Technology Indonesia', 'Leading business technology solutions', 'Jakarta Selatan', '+62-21-12345678', 'info@blitz.co.id', 'https://blitz.co.id', NOW(), true, false),
('DEMO', 'Demo Company Ltd', 'Demo company for testing', 'Bandung', '+62-22-87654321', 'demo@company.com', 'https://demo.com', NOW(), true, false),
('BIZSTACK', 'BizStack Corp', 'Business stack solutions', 'Surabaya', '+62-31-11111111', 'hello@bizstack.com', 'https://bizstack.com', NOW(), true, false);

-- Job Titles (using TenantId 1 and 2)
INSERT INTO organization_db."JobTitles" ("Id", "Title", "Description", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('44444444-4444-4444-4444-444444444444', 'CEO', 'Chief Executive Officer', NOW(), true, false, 1),
('55555555-5555-5555-5555-555555555555', 'CTO', 'Chief Technology Officer', NOW(), true, false, 1),
('66666666-6666-6666-6666-666666666666', 'Manager', 'Department Manager', NOW(), true, false, 1),
('77777777-7777-7777-7777-777777777777', 'Developer', 'Software Developer', NOW(), true, false, 1),
('88888888-8888-8888-8888-888888888888', 'Admin', 'System Administrator', NOW(), true, false, 2);

-- =============================================
-- 2. AUTH SERVICE - Users, Roles & Permissions
-- =============================================

-- Users (Auth) - CompanyId references Company.TenantId
INSERT INTO auth_db."Users" ("Id", "Username", "PasswordHash", "LoginFailCount", "LastLoginAt", "IsLocked", "CompanyId", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'admin', '$2a$11$rQw8qKqKqKqKqKqKqKqKqO', 0, NOW(), false, 1, NOW(), true, false, 1, 'system'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'manager', '$2a$11$rQw8qKqKqKqKqKqKqKqKqO', 0, NOW(), false, 1, NOW(), true, false, 1, 'admin'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'demo_user', '$2a$11$rQw8qKqKqKqKqKqKqKqKqO', 0, NOW(), false, 2, NOW(), true, false, 2, 'system'),
('dddddddd-dddd-dddd-dddd-dddddddddddd', 'bizstack_admin', '$2a$11$rQw8qKqKqKqKqKqKqKqKqO', 0, NOW(), false, 3, NOW(), true, false, 3, 'system');

-- Roles
INSERT INTO auth_db."Roles" ("Id", "Name", "Description", "CompanyId", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Super Admin', 'Full system access', 1, NOW(), true, false, 1, 'system'),
('ffffffff-ffff-ffff-ffff-ffffffffffff', 'Manager', 'Management access', 1, NOW(), true, false, 1, 'admin'),
('gggggggg-gggg-gggg-gggg-gggggggggggg', 'Employee', 'Basic employee access', 1, NOW(), true, false, 1, 'admin'),
('hhhhhhhh-hhhh-hhhh-hhhh-hhhhhhhhhhhh', 'Demo Admin', 'Demo company admin', 2, NOW(), true, false, 2, 'system');

-- Permissions
INSERT INTO auth_db."Permissions" ("Id", "Name", "Description", "Resource", "Action", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('iiiiiiii-iiii-iiii-iiii-iiiiiiiiiiii', 'users.read', 'Read users', 'users', 'read', NOW(), true, false, 1),
('jjjjjjjj-jjjj-jjjj-jjjj-jjjjjjjjjjjj', 'users.write', 'Write users', 'users', 'write', NOW(), true, false, 1),
('kkkkkkkk-kkkk-kkkk-kkkk-kkkkkkkkkkkk', 'products.read', 'Read products', 'products', 'read', NOW(), true, false, 1),
('llllllll-llll-llll-llll-llllllllllll', 'orders.read', 'Read orders', 'orders', 'read', NOW(), true, false, 1);

-- User Roles
INSERT INTO auth_db."UserRoles" ("Id", "UserId", "RoleId", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('mmmmmmmm-mmmm-mmmm-mmmm-mmmmmmmmmmmm', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', NOW(), true, false, 1),
('nnnnnnnn-nnnn-nnnn-nnnn-nnnnnnnnnnnn', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'ffffffff-ffff-ffff-ffff-ffffffffffff', NOW(), true, false, 1),
('oooooooo-oooo-oooo-oooo-oooooooooooo', 'cccccccc-cccc-cccc-cccc-cccccccccccc', 'hhhhhhhh-hhhh-hhhh-hhhh-hhhhhhhhhhhh', NOW(), true, false, 2);

-- =============================================
-- 3. USER SERVICE - User Profiles
-- =============================================

-- User Profiles (same Id as auth users)
INSERT INTO user_db."Users" ("Id", "Username", "Email", "FirstName", "LastName", "PhoneNumber", "Position", "Department", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'admin', 'admin@blitz.co.id', 'System', 'Administrator', '+62-812-3456-7890', 'CEO', 'Management', NOW(), true, false, 1, 'system'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'manager', 'manager@blitz.co.id', 'John', 'Manager', '+62-813-4567-8901', 'Manager', 'Operations', NOW(), true, false, 1, 'admin'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'demo_user', 'demo@company.com', 'Demo', 'User', '+62-814-5678-9012', 'Staff', 'General', NOW(), true, false, 2, 'system');

-- User Profiles Detail
INSERT INTO user_db."UserProfiles" ("Id", "UserId", "Bio", "Avatar", "DateOfBirth", "Gender", "Address", "City", "Country", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('pppppppp-pppp-pppp-pppp-pppppppppppp', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'System Administrator', '/avatars/admin.jpg', '1985-01-15', 'Male', 'Jakarta Selatan', 'Jakarta', 'Indonesia', NOW(), true, false, 1),
('qqqqqqqq-qqqq-qqqq-qqqq-qqqqqqqqqqqq', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Operations Manager', '/avatars/manager.jpg', '1990-05-20', 'Male', 'Jakarta Pusat', 'Jakarta', 'Indonesia', NOW(), true, false, 1);

-- =============================================
-- 4. PRODUCT SERVICE - Categories & Products
-- =============================================

-- Categories
INSERT INTO product_db."Categories" ("Id", "Name", "Description", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('rrrrrrrr-rrrr-rrrr-rrrr-rrrrrrrrrrrr', 'Electronics', 'Electronic devices and gadgets', NOW(), true, false, 1, 'admin'),
('ssssssss-ssss-ssss-ssss-ssssssssssss', 'Clothing', 'Fashion and apparel', NOW(), true, false, 1, 'admin'),
('tttttttt-tttt-tttt-tttt-tttttttttttt', 'Books', 'Books and publications', NOW(), true, false, 2, 'demo_user');

-- Products
INSERT INTO product_db."Products" ("Id", "Name", "Code", "Description", "Price", "Stock", "MinStock", "Unit", "CategoryId", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('uuuuuuuu-uuuu-uuuu-uuuu-uuuuuuuuuuuu', 'Laptop Gaming', 'LPT-001', 'High performance gaming laptop', 15000000, 10, 2, 'pcs', 'rrrrrrrr-rrrr-rrrr-rrrr-rrrrrrrrrrrr', NOW(), true, false, 1, 'admin'),
('vvvvvvvv-vvvv-vvvv-vvvv-vvvvvvvvvvvv', 'Smartphone', 'SPH-001', 'Latest Android smartphone', 8000000, 25, 5, 'pcs', 'rrrrrrrr-rrrr-rrrr-rrrr-rrrrrrrrrrrr', NOW(), true, false, 1, 'admin'),
('wwwwwwww-wwww-wwww-wwww-wwwwwwwwwwww', 'T-Shirt Premium', 'TSH-001', 'Premium cotton t-shirt', 150000, 50, 10, 'pcs', 'ssssssss-ssss-ssss-ssss-ssssssssssss', NOW(), true, false, 1, 'admin'),
('xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx', 'Programming Book', 'BK-001', 'Learn programming fundamentals', 250000, 20, 3, 'pcs', 'tttttttt-tttt-tttt-tttt-tttttttttttt', NOW(), true, false, 2, 'demo_user');

-- =============================================
-- 5. CUSTOMER SERVICE - Customer Groups & Customers
-- =============================================

-- Customer Groups
INSERT INTO customer_db."CustomerGroups" ("Id", "Name", "Description", "DiscountPercentage", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy', 'VIP', 'VIP customers with special privileges', 10.0, NOW(), true, false, 1, 'admin'),
('zzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzzzzzz', 'Regular', 'Regular customers', 0.0, NOW(), true, false, 1, 'admin'),
('11111111-2222-3333-4444-555555555555', 'Wholesale', 'Wholesale customers', 15.0, NOW(), true, false, 2, 'demo_user');

-- Customers
INSERT INTO customer_db."Customers" ("Id", "Name", "Email", "Phone", "Address", "City", "PostalCode", "CustomerType", "TotalSpent", "TotalOrders", "CustomerGroupId", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('22222222-3333-4444-5555-666666666666', 'Budi Santoso', 'budi@email.com', '+62-812-1111-2222', 'Jl. Sudirman No. 123', 'Jakarta', '12345', 'VIP', 5000000, 15, 'yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy', NOW(), true, false, 1, 'admin'),
('33333333-4444-5555-6666-777777777777', 'Siti Nurhaliza', 'siti@email.com', '+62-813-2222-3333', 'Jl. Thamrin No. 456', 'Jakarta', '12346', 'Regular', 1500000, 8, 'zzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzzzzzz', NOW(), true, false, 1, 'admin'),
('44444444-5555-6666-7777-888888888888', 'Demo Customer', 'customer@demo.com', '+62-814-3333-4444', 'Demo Address 789', 'Bandung', '40123', 'Wholesale', 3000000, 12, '11111111-2222-3333-4444-555555555555', NOW(), true, false, 2, 'demo_user');

-- =============================================
-- 6. TRANSACTION SERVICE - Orders & Payments
-- =============================================

-- Orders
INSERT INTO transaction_db."Orders" ("Id", "OrderNumber", "OrderDate", "Status", "SubTotal", "TaxAmount", "DiscountAmount", "TotalAmount", "CustomerId", "ShippingAddress", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('55555555-6666-7777-8888-999999999999', 'ORD-2024-001', NOW() - INTERVAL '5 days', 'Delivered', 15000000, 1500000, 500000, 16000000, '22222222-3333-4444-5555-666666666666', 'Jl. Sudirman No. 123, Jakarta', NOW() - INTERVAL '5 days', true, false, 1, 'admin'),
('66666666-7777-8888-9999-aaaaaaaaaaaa', 'ORD-2024-002', NOW() - INTERVAL '2 days', 'Processing', 8000000, 800000, 0, 8800000, '33333333-4444-5555-6666-777777777777', 'Jl. Thamrin No. 456, Jakarta', NOW() - INTERVAL '2 days', true, false, 1, 'admin'),
('77777777-8888-9999-aaaa-bbbbbbbbbbbb', 'DEMO-001', NOW() - INTERVAL '1 day', 'Confirmed', 250000, 25000, 37500, 237500, '44444444-5555-6666-7777-888888888888', 'Demo Address 789, Bandung', NOW() - INTERVAL '1 day', true, false, 2, 'demo_user');

-- Order Items
INSERT INTO transaction_db."OrderItems" ("Id", "OrderId", "ProductId", "ProductName", "Quantity", "UnitPrice", "TotalPrice", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('88888888-9999-aaaa-bbbb-cccccccccccc', '55555555-6666-7777-8888-999999999999', 'uuuuuuuu-uuuu-uuuu-uuuu-uuuuuuuuuuuu', 'Laptop Gaming', 1, 15000000, 15000000, NOW() - INTERVAL '5 days', true, false, 1),
('99999999-aaaa-bbbb-cccc-dddddddddddd', '66666666-7777-8888-9999-aaaaaaaaaaaa', 'vvvvvvvv-vvvv-vvvv-vvvv-vvvvvvvvvvvv', 'Smartphone', 1, 8000000, 8000000, NOW() - INTERVAL '2 days', true, false, 1),
('aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee', '77777777-8888-9999-aaaa-bbbbbbbbbbbb', 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx', 'Programming Book', 1, 250000, 250000, NOW() - INTERVAL '1 day', true, false, 2);

-- Payments
INSERT INTO transaction_db."Payments" ("Id", "OrderId", "PaymentMethod", "Amount", "Status", "PaymentDate", "TransactionId", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('bbbbbbbb-cccc-dddd-eeee-ffffffffffff', '55555555-6666-7777-8888-999999999999', 'Bank Transfer', 16000000, 'Completed', NOW() - INTERVAL '4 days', 'TXN-001', NOW() - INTERVAL '4 days', true, false, 1),
('cccccccc-dddd-eeee-ffff-gggggggggggg', '66666666-7777-8888-9999-aaaaaaaaaaaa', 'Credit Card', 8800000, 'Pending', NOW() - INTERVAL '1 day', 'TXN-002', NOW() - INTERVAL '1 day', true, false, 1);

-- =============================================
-- 7. NOTIFICATION SERVICE - Notifications
-- =============================================

-- Notifications
INSERT INTO notification_db."Notifications" ("Id", "Type", "Recipient", "Subject", "Message", "Status", "SentAt", "RelatedEntityId", "RelatedEntityType", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('dddddddd-eeee-ffff-gggg-hhhhhhhhhhhh', 'Email', 'budi@email.com', 'Order Confirmation', 'Your order ORD-2024-001 has been confirmed', 'Sent', NOW() - INTERVAL '5 days', '55555555-6666-7777-8888-999999999999', 'Order', NOW() - INTERVAL '5 days', true, false, 1),
('eeeeeeee-ffff-gggg-hhhh-iiiiiiiiiiii', 'SMS', '+62-813-2222-3333', 'Order Update', 'Your order is being processed', 'Sent', NOW() - INTERVAL '2 days', '66666666-7777-8888-9999-aaaaaaaaaaaa', 'Order', NOW() - INTERVAL '2 days', true, false, 1),
('ffffffff-gggg-hhhh-iiii-jjjjjjjjjjjj', 'WhatsApp', '+62-814-3333-4444', 'Welcome', 'Welcome to our demo system!', 'Delivered', NOW() - INTERVAL '1 day', '44444444-5555-6666-7777-888888888888', 'Customer', NOW() - INTERVAL '1 day', true, false, 2);

-- =============================================
-- 8. FILE STORAGE SERVICE - File Records
-- =============================================

-- File Records
INSERT INTO file_storage_db."FileRecords" ("Id", "FileName", "OriginalFileName", "FilePath", "FileSize", "ContentType", "UploadedBy", "CreatedAt", "IsActive", "IsDeleted", "TenantId") VALUES
('gggggggg-hhhh-iiii-jjjj-kkkkkkkkkkkk', 'avatar_admin.jpg', 'admin-photo.jpg', '/uploads/avatars/avatar_admin.jpg', 245760, 'image/jpeg', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW() - INTERVAL '10 days', true, false, 1),
('hhhhhhhh-iiii-jjjj-kkkk-llllllllllll', 'product_laptop.jpg', 'gaming-laptop.jpg', '/uploads/products/product_laptop.jpg', 512000, 'image/jpeg', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW() - INTERVAL '7 days', true, false, 1),
('iiiiiiii-jjjj-kkkk-llll-mmmmmmmmmmmm', 'invoice_001.pdf', 'invoice-ORD-2024-001.pdf', '/uploads/invoices/invoice_001.pdf', 102400, 'application/pdf', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW() - INTERVAL '5 days', true, false, 1);

-- =============================================
-- 9. SETTINGS SERVICE - System Settings & Menu Items
-- =============================================

-- System Settings
INSERT INTO settings_db."SystemSettings" ("Id", "Key", "Value", "Description", "Category", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('jjjjjjjj-kkkk-llll-mmmm-nnnnnnnnnnnn', 'company.name', 'PT Blitz Technology Indonesia', 'Company name for tenant 1', 'Company', NOW(), true, false, 1, 'admin'),
('kkkkkkkk-llll-mmmm-nnnn-oooooooooooo', 'system.timezone', 'Asia/Jakarta', 'System timezone', 'System', NOW(), true, false, 1, 'admin'),
('llllllll-mmmm-nnnn-oooo-pppppppppppp', 'email.smtp.host', 'smtp.gmail.com', 'SMTP server host', 'Email', NOW(), true, false, 1, 'admin'),
('mmmmmmmm-nnnn-oooo-pppp-qqqqqqqqqqqq', 'demo.setting', 'demo_value', 'Demo setting for tenant 2', 'Demo', NOW(), true, false, 2, 'demo_user');

-- Menu Items
INSERT INTO settings_db."MenuItems" ("Id", "Name", "Path", "Icon", "ParentId", "SortOrder", "IsVisible", "CreatedAt", "IsActive", "IsDeleted", "TenantId", "CreatedBy") VALUES
('nnnnnnnn-oooo-pppp-qqqq-rrrrrrrrrrrr', 'Dashboard', '/dashboard', 'fas fa-tachometer-alt', NULL, 1, true, NOW(), true, false, 1, 'admin'),
('oooooooo-pppp-qqqq-rrrr-ssssssssssss', 'Products', '/products', 'fas fa-box', NULL, 2, true, NOW(), true, false, 1, 'admin'),
('pppppppp-qqqq-rrrr-ssss-tttttttttttt', 'Customers', '/customers', 'fas fa-users', NULL, 3, true, NOW(), true, false, 1, 'admin'),
('qqqqqqqq-rrrr-ssss-tttt-uuuuuuuuuuuu', 'Orders', '/orders', 'fas fa-shopping-cart', NULL, 4, true, NOW(), true, false, 1, 'admin'),
('rrrrrrrr-ssss-tttt-uuuu-vvvvvvvvvvvv', 'Settings', '/settings', 'fas fa-cogs', NULL, 5, true, NOW(), true, false, 1, 'admin');

-- =============================================
-- Summary:
-- Company.TenantId: 1=BLITZ, 2=DEMO, 3=BIZSTACK (auto-generated)
-- All other entities use TenantId as foreign key reference
-- =============================================