using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.Models;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CostCentersController : ControllerBase
{
    private readonly OrganizationDbContext _context;

    public CostCentersController(OrganizationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCostCenters()
    {
        var costCenters = await _context.CostCenters
            .Include(cc => cc.Division)
            .ToListAsync();
        return Ok(ApiResponse<List<CostCenter>>.Success(costCenters));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCostCenter(Guid id)
    {
        var costCenter = await _context.CostCenters
            .Include(cc => cc.Division)
            .FirstOrDefaultAsync(cc => cc.Id == id);
        
        if (costCenter == null)
            return NotFound(ApiResponse<CostCenter>.Error("Cost center not found"));
        
        return Ok(ApiResponse<CostCenter>.Success(costCenter));
    }

    [HttpGet("division/{divisionId}")]
    public async Task<IActionResult> GetCostCentersByDivision(Guid divisionId)
    {
        var costCenters = await _context.CostCenters
            .Where(cc => cc.Division.Id == divisionId)
            .Include(cc => cc.Division)
            .ToListAsync();
        
        return Ok(ApiResponse<List<CostCenter>>.Success(costCenters));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCostCenter([FromBody] CreateCostCenterDto dto)
    {
        var division = await _context.Divisions.FindAsync(dto.DivisionId);
        if (division == null)
            return BadRequest(ApiResponse<CostCenter>.Error("Division not found"));

        var costCenter = new CostCenter
        {
            Code = dto.Code,
            Name = dto.Name,
            Division = division
        };

        _context.CostCenters.Add(costCenter);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetCostCenter), new { id = costCenter.Id }, ApiResponse<CostCenter>.Success(costCenter));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCostCenter(Guid id, [FromBody] UpdateCostCenterDto dto)
    {
        var costCenter = await _context.CostCenters.FindAsync(id);
        if (costCenter == null)
            return NotFound(ApiResponse<CostCenter>.Error("Cost center not found"));

        if (!string.IsNullOrEmpty(dto.Code)) costCenter.Code = dto.Code;
        if (!string.IsNullOrEmpty(dto.Name)) costCenter.Name = dto.Name;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<CostCenter>.Success(costCenter));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCostCenter(Guid id)
    {
        var costCenter = await _context.CostCenters.FindAsync(id);
        if (costCenter == null)
            return NotFound(ApiResponse<CostCenter>.Error("Cost center not found"));

        _context.CostCenters.Remove(costCenter);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Cost center deleted successfully"));
    }
}

public class CreateCostCenterDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid DivisionId { get; set; }
}

public class UpdateCostCenterDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}