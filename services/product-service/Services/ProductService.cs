using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Models;
using SharedLibrary.DTOs;

namespace ProductService.Services;

public class ProductService : IProductService
{
    private readonly ProductDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(ProductDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync(int? tenantId = null)
    {
        var query = _context.Products.Where(x => !x.IsDeleted)
            .Include(p => p.Category)
            .Where(p => p.IsActive);
            
        if (tenantId.HasValue)
            query = query.Where(p => p.TenantId == tenantId.Value);
            
        var products = await query.ToListAsync();
        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return new ApiResponse<List<ProductDto>> { Data = productDtos, IsSuccess = true, Message = "Products retrieved successfully" };
    }

    public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(Guid id)
    {
        var product = await _context.Products.Where(x => !x.IsDeleted)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return new ApiResponse<ProductDto> { Data = null, IsSuccess = false, Message = "Product not found" };

        var productDto = _mapper.Map<ProductDto>(product);
        return new ApiResponse<ProductDto> { Data = productDto, IsSuccess = true, Message = "Product retrieved successfully" };
    }

    public async Task<ApiResponse<List<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId)
    {
        var products = await _context.Products.Where(x => !x.IsDeleted)
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return new ApiResponse<List<ProductDto>> { Data = productDtos, IsSuccess = true, Message = "Products by category retrieved" };
    }

    public async Task<ApiResponse<List<ProductDto>>> SearchProductsAsync(string searchTerm)
    {
        var products = await _context.Products.Where(x => !x.IsDeleted)
            .Include(p => p.Category)
            .Where(p => p.IsActive && 
                       (p.Name.Contains(searchTerm) || 
                        p.Code.Contains(searchTerm) ||
                        (p.Description != null && p.Description.Contains(searchTerm))))
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return new ApiResponse<List<ProductDto>> { Data = productDtos, IsSuccess = true, Message = "Products found" };
    }

    public async Task<ApiResponse<List<ProductDto>>> GetLowStockProductsAsync()
    {
        var products = await _context.Products.Where(x => !x.IsDeleted)
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.Stock <= p.MinStock)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return new ApiResponse<List<ProductDto>> { Data = productDtos, IsSuccess = true, Message = "Low stock products retrieved" };
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto, int? tenantId = null)
    {
        var product = _mapper.Map<Product>(createProductDto);
        product.TenantId = tenantId;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(product);
        return new ApiResponse<ProductDto> { Data = productDto, IsSuccess = true, Message = "Product created successfully" };
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return new ApiResponse<ProductDto> { Data = null, IsSuccess = false, Message = "Product not found" };

        _mapper.Map(updateProductDto, product);
        await _context.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(product);
        return new ApiResponse<ProductDto> { Data = productDto, IsSuccess = true, Message = "Product updated successfully" };
    }

    public async Task<ApiResponse<string>> DeleteProductAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Product not found" };

        product.IsActive = false;
        await _context.SaveChangesAsync();

        return new ApiResponse<string> { Data = "Product deleted successfully", IsSuccess = true, Message = "Product deleted successfully" };
    }

    public async Task<ApiResponse<string>> UpdateStockAsync(Guid id, int quantity)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Product not found" };

        product.Stock += quantity;
        await _context.SaveChangesAsync();

        return new ApiResponse<string> { Data = $"Stock updated. New stock: {product.Stock}", IsSuccess = true, Message = "Stock updated successfully" };
    }
}