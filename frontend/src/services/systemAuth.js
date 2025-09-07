// System Authentication Service
class SystemAuth {
  static SYSTEM_CREDENTIALS = {
    username: 'system_frontend',
    password: 'SysF3nt3nd2024!@#'
  };

  static systemToken = null;
  static tokenExpiry = null;

  // Get system token for backend communication
  static async getSystemToken() {
    // Check if token is still valid
    if (this.systemToken && this.tokenExpiry && Date.now() < this.tokenExpiry) {
      return this.systemToken;
    }

    try {
      const response = await fetch('http://localhost:5001/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(this.SYSTEM_CREDENTIALS)
      });

      const result = await response.json();
      
      if (result.isSuccess && result.data?.accessToken) {
        this.systemToken = result.data.accessToken;
        // Set expiry to 50 minutes (tokens usually expire in 1 hour)
        this.tokenExpiry = Date.now() + (50 * 60 * 1000);
        return this.systemToken;
      }
      
      throw new Error('Failed to authenticate system user');
    } catch (error) {
      console.error('System authentication failed:', error);
      throw error;
    }
  }

  // Make authenticated request to backend
  static async makeAuthenticatedRequest(url, options = {}) {
    const token = await this.getSystemToken();
    
    return fetch(url, {
      ...options,
      headers: {
        ...options.headers,
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
  }

  // Clear system token
  static clearSystemToken() {
    this.systemToken = null;
    this.tokenExpiry = null;
  }
}

export default SystemAuth;