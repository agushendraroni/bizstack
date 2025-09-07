// Sample data for BizStack demo
import { productApi, customerApi, notificationApi } from '../api';

export const seedSampleData = async () => {
  console.log('🌱 Seeding sample data...');
  
  try {
    // Sample products
    const sampleProducts = [
      {
        name: "Laptop Dell XPS 13",
        code: "LAPTOP001",
        description: "High-performance ultrabook for professionals",
        price: 1299.99,
        stock: 15,
        minStock: 5,
        isActive: true
      },
      {
        name: "Wireless Mouse",
        code: "MOUSE001", 
        description: "Ergonomic wireless mouse with precision tracking",
        price: 29.99,
        stock: 2,
        minStock: 10,
        isActive: true
      },
      {
        name: "USB-C Hub",
        code: "HUB001",
        description: "Multi-port USB-C hub with HDMI and USB 3.0",
        price: 49.99,
        stock: 25,
        minStock: 5,
        isActive: true
      },
      {
        name: "Bluetooth Headphones",
        code: "HEADPHONE001",
        description: "Noise-cancelling wireless headphones",
        price: 199.99,
        stock: 8,
        minStock: 3,
        isActive: true
      },
      {
        name: "Mechanical Keyboard",
        code: "KEYBOARD001",
        description: "RGB mechanical gaming keyboard",
        price: 129.99,
        stock: 1,
        minStock: 5,
        isActive: true
      }
    ];

    // Sample customers
    const sampleCustomers = [
      {
        name: "John Smith",
        email: "john.smith@email.com",
        phone: "+1-555-0101",
        address: "123 Main St, New York, NY 10001",
        customerType: "Regular",
        isActive: true
      },
      {
        name: "Sarah Johnson",
        email: "sarah.johnson@email.com", 
        phone: "+1-555-0102",
        address: "456 Oak Ave, Los Angeles, CA 90210",
        customerType: "VIP",
        isActive: true
      },
      {
        name: "Tech Solutions Inc",
        email: "orders@techsolutions.com",
        phone: "+1-555-0103", 
        address: "789 Business Blvd, Chicago, IL 60601",
        customerType: "Corporate",
        isActive: true
      },
      {
        name: "Mike Wilson",
        email: "mike.wilson@email.com",
        phone: "+1-555-0104",
        address: "321 Pine St, Seattle, WA 98101", 
        customerType: "Wholesale",
        isActive: true
      },
      {
        name: "Emma Davis",
        email: "emma.davis@email.com",
        phone: "+1-555-0105",
        address: "654 Elm Dr, Miami, FL 33101",
        customerType: "Regular", 
        isActive: true
      }
    ];

    // Create products
    console.log('📦 Creating sample products...');
    const createdProducts = [];
    for (const product of sampleProducts) {
      try {
        const result = await productApi.createProduct(product);
        if (result.success) {
          createdProducts.push(result.data);
          console.log(`✅ Created product: ${product.name}`);
        } else {
          console.log(`⚠️  Failed to create product: ${product.name} - ${result.message}`);
        }
      } catch (error) {
        console.log(`❌ Error creating product: ${product.name} - ${error.message}`);
      }
    }

    // Create customers  
    console.log('👥 Creating sample customers...');
    const createdCustomers = [];
    for (const customer of sampleCustomers) {
      try {
        const result = await customerApi.createCustomer(customer);
        if (result.success) {
          createdCustomers.push(result.data);
          console.log(`✅ Created customer: ${customer.name}`);
        } else {
          console.log(`⚠️  Failed to create customer: ${customer.name} - ${result.message}`);
        }
      } catch (error) {
        console.log(`❌ Error creating customer: ${customer.name} - ${error.message}`);
      }
    }

    // Create sample notifications
    console.log('🔔 Creating sample notifications...');
    const userId = localStorage.getItem('userId');
    if (userId) {
      const sampleNotifications = [
        {
          userId: userId,
          title: "Welcome to BizStack!",
          message: "Your business management system is ready to use.",
          type: "info"
        },
        {
          userId: userId,
          title: "Low Stock Alert",
          message: "Some products are running low on stock. Check inventory.",
          type: "warning"
        },
        {
          userId: userId,
          title: "New Order Received",
          message: "You have received a new order from a customer.",
          type: "success"
        }
      ];

      for (const notification of sampleNotifications) {
        try {
          const result = await notificationApi.sendNotification(notification);
          if (result.success) {
            console.log(`✅ Created notification: ${notification.title}`);
          }
        } catch (error) {
          console.log(`❌ Error creating notification: ${notification.title}`);
        }
      }
    }

    console.log('🎉 Sample data seeding completed!');
    console.log(`📊 Summary:`);
    console.log(`   - Products: ${createdProducts.length}/${sampleProducts.length}`);
    console.log(`   - Customers: ${createdCustomers.length}/${sampleCustomers.length}`);
    
    return {
      success: true,
      products: createdProducts,
      customers: createdCustomers,
      message: 'Sample data created successfully'
    };

  } catch (error) {
    console.error('❌ Error seeding sample data:', error);
    return {
      success: false,
      message: error.message || 'Failed to seed sample data'
    };
  }
};

export const clearSampleData = async () => {
  console.log('🧹 Clearing sample data...');
  // Implementation for clearing sample data if needed
  console.log('✅ Sample data cleared');
};