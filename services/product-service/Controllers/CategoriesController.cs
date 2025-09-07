using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Models;
using SharedLibrary.DTOs;

namespace ProductService.Controllers;

[ApiController]
[ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ProductDbContext _context;

    public CategoriesController(ProductDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories
            .Include(c => c.ParentCategory)
            .Where(c => c.IsActive)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                CreatedAt = c.CreatedAt,
                ProductCount = c.Products.Count(p => p.IsActive)
            })
            .ToListAsync();

        return Ok(ApiResponse<List<CategoryDto>>.Success(categories));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await _context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound(ApiResponse<CategoryDto>.Error("Category not found"));

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            IsActive = category.IsActive,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = category.ParentCategory?.Name,
            CreatedAt = category.CreatedAt,
            ProductCount = category.Products.Count(p => p.IsActive)
        };

        return Ok(ApiResponse<CategoryDto>.Success(categoryDto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            ParentCategoryId = dto.ParentCategoryId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            IsActive = category.IsActive,
            ParentCategoryId = category.ParentCategoryId,
            CreatedAt = category.CreatedAt,
            ProductCount = 0
        };

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, ApiResponse<CategoryDto>.Success(categoryDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(ApiResponse<CategoryDto>.Error("Category not found"));

        if (!string.IsNullOrEmpty(dto.Name)) category.Name = dto.Name;
        if (dto.Description != null) category.Description = dto.Description;
        if (dto.ImageUrl != null) category.ImageUrl = dto.ImageUrl;
        if (dto.ParentCategoryId.HasValue) category.ParentCategoryId = dto.ParentCategoryId;
        if (dto.IsActive.HasValue) category.IsActive = dto.IsActive.Value;

        await _context.SaveChangesAsync();

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            IsActive = category.IsActive,
            ParentCategoryId = category.ParentCategoryId,
            CreatedAt = category.CreatedAt,
            ProductCount = await _context.Products.CountAsync(p => p.CategoryId == id && p.IsActive)
        };

        return Ok(ApiResponse<CategoryDto>.Success(categoryDto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(ApiResponse<CategoryDto>.Error("Category not found"));

        category.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success("Category deleted successfully"));
    }

    private int? GetTenantId()
    {
        if (Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader) && 
            int.TryParse(tenantIdHeader.FirstOrDefault(), out var tenantId))
        {
            return tenantId;
        }
        return null;
    }

    private Guid? GetUserId()
    {
        if (Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) && 
            Guid.TryParse(userIdHeader.FirstOrDefault(), out var userId))
        {
            return userId;
        }
        return null;
    }
}
