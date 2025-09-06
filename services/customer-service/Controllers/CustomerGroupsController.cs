using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerService.Data;
using CustomerService.DTOs;
using CustomerService.Models;
using SharedLibrary.DTOs;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerGroupsController : ControllerBase
{
    private readonly CustomerDbContext _context;

    public CustomerGroupsController(CustomerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerGroups()
    {
        var groups = await _context.CustomerGroups
            .Where(cg => cg.IsActive)
            .Select(cg => new CustomerGroupDto
            {
                Id = cg.Id,
                Name = cg.Name,
                Description = cg.Description,
                DiscountPercentage = cg.DiscountPercentage,
                MinimumSpent = cg.MinimumSpent,
                Color = cg.Color,
                CreatedAt = cg.CreatedAt,
                CustomerCount = cg.Customers.Count(c => c.IsActive),
                IsActive = cg.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<CustomerGroupDto>>.Success(groups));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerGroup(Guid id)
    {
        var group = await _context.CustomerGroups
            .Include(cg => cg.Customers)
            .FirstOrDefaultAsync(cg => cg.Id == id);

        if (group == null)
            return NotFound(ApiResponse<CustomerGroupDto>.Error("Customer group not found"));

        var groupDto = new CustomerGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            DiscountPercentage = group.DiscountPercentage,
            MinimumSpent = group.MinimumSpent,
            Color = group.Color,
            CreatedAt = group.CreatedAt,
            CustomerCount = group.Customers.Count(c => c.IsActive),
            IsActive = group.IsActive
        };

        return Ok(ApiResponse<CustomerGroupDto>.Success(groupDto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerGroup([FromBody] CreateCustomerGroupDto dto)
    {
        var group = new CustomerGroup
        {
            Name = dto.Name,
            Description = dto.Description,
            DiscountPercentage = dto.DiscountPercentage,
            MinimumSpent = dto.MinimumSpent,
            Color = dto.Color
        };

        _context.CustomerGroups.Add(group);
        await _context.SaveChangesAsync();

        var groupDto = new CustomerGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            DiscountPercentage = group.DiscountPercentage,
            MinimumSpent = group.MinimumSpent,
            Color = group.Color,
            CreatedAt = group.CreatedAt,
            CustomerCount = 0,
            IsActive = group.IsActive
        };

        return CreatedAtAction(nameof(GetCustomerGroup), new { id = group.Id }, ApiResponse<CustomerGroupDto>.Success(groupDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerGroup(Guid id, [FromBody] UpdateCustomerGroupDto dto)
    {
        var group = await _context.CustomerGroups.FindAsync(id);
        if (group == null)
            return NotFound(ApiResponse<CustomerGroupDto>.Error("Customer group not found"));

        if (!string.IsNullOrEmpty(dto.Name)) group.Name = dto.Name;
        if (dto.Description != null) group.Description = dto.Description;
        if (dto.DiscountPercentage.HasValue) group.DiscountPercentage = dto.DiscountPercentage;
        if (dto.MinimumSpent.HasValue) group.MinimumSpent = dto.MinimumSpent;
        if (dto.Color != null) group.Color = dto.Color;

        await _context.SaveChangesAsync();

        var groupDto = new CustomerGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            DiscountPercentage = group.DiscountPercentage,
            MinimumSpent = group.MinimumSpent,
            Color = group.Color,
            CreatedAt = group.CreatedAt,
            CustomerCount = await _context.Customers.CountAsync(c => c.CustomerGroupId == id && c.IsActive),
            IsActive = group.IsActive
        };

        return Ok(ApiResponse<CustomerGroupDto>.Success(groupDto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerGroup(Guid id)
    {
        var group = await _context.CustomerGroups.FindAsync(id);
        if (group == null)
            return NotFound(ApiResponse<CustomerGroupDto>.Error("Customer group not found"));

        group.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success("Customer group deleted successfully"));
    }
}
