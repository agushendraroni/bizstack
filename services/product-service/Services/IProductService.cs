using ProductService.DTOs;
using SharedLibrary.DTOs;

namespace ProductService.Services;

public interface IProductService
{
    Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync(int? tenantId = null);
    Task<ApiResponse<ProductDto>> GetProductByIdAsync(Guid id);
    Task<ApiResponse<List<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId);
    Task<ApiResponse<List<ProductDto>>> SearchProductsAsync(string searchTerm);
    Task<ApiResponse<List<ProductDto>>> GetLowStockProductsAsync();
    Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto, int? tenantId = null);
    Task<ApiResponse<ProductDto>> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto);
    Task<ApiResponse<string>> DeleteProductAsync(Guid id);
    Task<ApiResponse<string>> UpdateStockAsync(Guid id, int quantity);
}