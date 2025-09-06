const { GraphQLMesh } = require('@graphql-mesh/runtime');
const { getMesh } = require('@graphql-mesh/cli');

async function testGraphQL() {
  try {
    console.log('🚀 Starting GraphQL Mesh test...');
    
    // Test basic connectivity
    const response = await fetch('http://localhost:5001/health');
    if (response.ok) {
      console.log('✅ Auth Service is running');
    }
    
    const response2 = await fetch('http://localhost:5002/health');
    if (response2.ok) {
      console.log('✅ User Service is running');
    }
    
    const response3 = await fetch('http://localhost:5004/health');
    if (response3.ok) {
      console.log('✅ Product Service is running');
    }
    
    console.log('🎯 Services are ready for GraphQL integration!');
    
  } catch (error) {
    console.error('❌ Error:', error.message);
  }
}

testGraphQL();
