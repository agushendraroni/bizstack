// Product API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';
import {
  GET_ALL_PRODUCTS_QUERY,
  GET_PRODUCT_BY_ID_QUERY,
  GET_LOW_STOCK_PRODUCTS_QUERY,
  CREATE_PRODUCT_MUTATION,
  UPDATE_PRODUCT_MUTATION,
  DELETE_PRODUCT_MUTATION,
  UPDATE_STOCK_MUTATION
} from '../../services/api/queries';

class ProductAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get all products
  async getAllProducts() {
    try {
      const response = await this.client.query(GET_ALL_PRODUCTS_QUERY);
      const productData = response.ProductService_getProductsControllerGetAllProducts;
      
      return {
        success: productData.isSuccess,
        data: productData.data || [],
        message: productData.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch products'
      };
    }
  }

  // Get product by ID
  async getProductById(id) {
    try {
      const response = await this.client.query(GET_PRODUCT_BY_ID_QUERY, { id });
      const productData = response.ProductService_getProductsControllerGetProductById;
      
      return {
        success: productData.isSuccess,
        data: productData.data,
        message: productData.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to fetch product'
      };
    }
  }

  // Get low stock products
  async getLowStockProducts() {
    try {
      const response = await this.client.query(GET_LOW_STOCK_PRODUCTS_QUERY);
      const productData = response.ProductService_getProductsControllerGetLowStockProducts;
      
      return {
        success: productData.isSuccess,
        data: productData.data || [],
        message: productData.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch low stock products'
      };
    }
  }

  // Create product
  async createProduct(productData) {
    try {
      const response = await this.client.mutate(CREATE_PRODUCT_MUTATION, {
        createProductDto: productData
      });
      const result = response.ProductService_postProductsControllerCreateProduct;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to create product'
      };
    }
  }

  // Update product
  async updateProduct(id, productData) {
    try {
      const response = await this.client.mutate(UPDATE_PRODUCT_MUTATION, {
        id,
        updateProductDto: productData
      });
      const result = response.ProductService_putProductsControllerUpdateProduct;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to update product'
      };
    }
  }

  // Delete product
  async deleteProduct(id) {
    try {
      const response = await this.client.mutate(DELETE_PRODUCT_MUTATION, { id });
      const result = response.ProductService_deleteProductsControllerDeleteProduct;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to delete product'
      };
    }
  }

  // Update stock
  async updateStock(id, quantity) {
    try {
      const response = await this.client.mutate(UPDATE_STOCK_MUTATION, {
        id,
        updateStockDto: { quantity }
      });
      const result = response.ProductService_patchProductsControllerUpdateStock;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to update stock'
      };
    }
  }
}

export default new ProductAPI();