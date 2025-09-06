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

    public async Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return ApiResponse<List<ProductDto>>.Success(productDtos);
    }

    public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return ApiResponse<ProductDto>.Error("Product not found");

        var productDto = _mapper.Map<ProductDto>(product);
        return ApiResponse<ProductDto>.Success(productDto);
    }

    public async Task<ApiResponse<List<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId)
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return ApiResponse<List<ProductDto>>.Success(productDtos);
    }

    public async Task<ApiResponse<List<ProductDto>>> SearchProductsAsync(string searchTerm)
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && 
                       (p.Name.Contains(searchTerm) || 
                        p.Code.Contains(searchTerm) ||
                        (p.Description != null && p.Description.Contains(searchTerm))))
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return ApiResponse<List<ProductDto>>.Success(productDtos);
    }

    public async Task<ApiResponse<List<ProductDto>>> GetLowStockProductsAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.Stock <= p.MinStock)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return ApiResponse<List<ProductDto>>.Success(productDtos);
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(product);
        return ApiResponse<ProductDto>.Success(productDto);
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return ApiResponse<ProductDto>.Error("Product not found");

        _mapper.Map(updateProductDto, product);
        await _context.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(product);
        return ApiResponse<ProductDto>.Success(productDto);
    }

    public async Task<ApiResponse<string>> DeleteProductAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return ApiResponse<string>.Error("Product not found");

        product.IsActive = false;
        await _context.SaveChangesAsync();

        return ApiResponse<string>.Success("Product deleted successfully");
    }

    public async Task<ApiResponse<string>> UpdateStockAsync(Guid id, int quantity)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return ApiResponse<string>.Error("Product not found");

        product.Stock += quantity;
        await _context.SaveChangesAsync();

        return ApiResponse<string>.Success($"Stock updated. New stock: {product.Stock}");
    }
}
