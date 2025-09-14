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
public class DivisionsController : ControllerBase
{
    private readonly OrganizationDbContext _context;

    public DivisionsController(OrganizationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDivisions()
    {
        var tenantId = GetTenantId();
        var query = _context.Divisions.Include(d => d.Company).AsQueryable();
        
        if (tenantId.HasValue)
            query = query.Where(d => d.TenantId == tenantId.Value);
            
        var divisions = await query.ToListAsync();
        return Ok(ApiResponse<List<Division>>.Success(divisions));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDivision(Guid id)
    {
        var division = await _context.Divisions
            .Include(d => d.Company)
            .FirstOrDefaultAsync(d => d.Id == id);
        
        if (division == null)
            return NotFound(ApiResponse<Division>.Error("Division not found"));
        
        return Ok(ApiResponse<Division>.Success(division));
    }

    [HttpGet("company/{tenantId}")]
    public async Task<IActionResult> GetDivisionsByCompany(int tenantId)
    {
        var divisions = await _context.Divisions
            .Where(d => d.TenantId == tenantId)
            .Include(d => d.Company)
            .ToListAsync();
        
        return Ok(ApiResponse<List<Division>>.Success(divisions));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDivision([FromBody] CreateDivisionDto dto)
    {
        var tenantId = GetTenantId();
        var division = new Division
        {
            Name = dto.Name,
            TenantId = dto.TenantId ?? tenantId ?? 0
        };

        _context.Divisions.Add(division);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetDivision), new { id = division.Id }, ApiResponse<Division>.Success(division));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDivision(Guid id, [FromBody] UpdateDivisionDto dto)
    {
        var division = await _context.Divisions.FindAsync(id);
        if (division == null)
            return NotFound(ApiResponse<Division>.Error("Division not found"));

        if (!string.IsNullOrEmpty(dto.Name)) division.Name = dto.Name;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Division>.Success(division));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDivision(Guid id)
    {
        var division = await _context.Divisions.FindAsync(id);
        if (division == null)
            return NotFound(ApiResponse<Division>.Error("Division not found"));

        _context.Divisions.Remove(division);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Division deleted successfully"));
    }

    private int? GetTenantId()
    {
        return null;
    }
}

public class CreateDivisionDto
{
    public string Name { get; set; } = string.Empty;
    public int? TenantId { get; set; }
}

public class UpdateDivisionDto
{
    public string? Name { get; set; }

    private Guid? GetUserId()
    {
        return null;
    }
}
