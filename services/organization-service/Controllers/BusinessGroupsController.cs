using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.Models;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BusinessGroupsController : ControllerBase
{
    private readonly OrganizationDbContext _context;

    public BusinessGroupsController(OrganizationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBusinessGroups()
    {
        var groups = await _context.BusinessGroups
            .Include(bg => bg.Companies)
            .ToListAsync();
        return Ok(ApiResponse<List<BusinessGroup>>.Success(groups));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBusinessGroup(Guid id)
    {
        var group = await _context.BusinessGroups
            .Include(bg => bg.Companies)
            .FirstOrDefaultAsync(bg => bg.Id == id);
        
        if (group == null)
            return NotFound(ApiResponse<BusinessGroup>.Error("Business group not found"));
        
        return Ok(ApiResponse<BusinessGroup>.Success(group));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBusinessGroup([FromBody] CreateBusinessGroupDto dto)
    {
        var group = new BusinessGroup
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.BusinessGroups.Add(group);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetBusinessGroup), new { id = group.Id }, ApiResponse<BusinessGroup>.Success(group));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBusinessGroup(Guid id, [FromBody] UpdateBusinessGroupDto dto)
    {
        var group = await _context.BusinessGroups.FindAsync(id);
        if (group == null)
            return NotFound(ApiResponse<BusinessGroup>.Error("Business group not found"));

        if (!string.IsNullOrEmpty(dto.Name)) group.Name = dto.Name;
        if (dto.Description != null) group.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<BusinessGroup>.Success(group));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBusinessGroup(Guid id)
    {
        var group = await _context.BusinessGroups.FindAsync(id);
        if (group == null)
            return NotFound(ApiResponse<BusinessGroup>.Error("Business group not found"));

        _context.BusinessGroups.Remove(group);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Business group deleted successfully"));
    }
}

public class CreateBusinessGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateBusinessGroupDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    private Guid? GetUserId()
    {
        return null;
    }
}