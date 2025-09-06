using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.Models;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchesController : ControllerBase
{
    private readonly OrganizationDbContext _context;

    public BranchesController(OrganizationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await _context.Branches
            .Include(b => b.Company)
            .ToListAsync();
        return Ok(ApiResponse<List<Branch>>.Success(branches));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBranch(Guid id)
    {
        var branch = await _context.Branches
            .Include(b => b.Company)
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (branch == null)
            return NotFound(ApiResponse<Branch>.Error("Branch not found"));
        
        return Ok(ApiResponse<Branch>.Success(branch));
    }

    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetBranchesByCompany(Guid companyId)
    {
        var branches = await _context.Branches
            .Where(b => b.CompanyId == companyId)
            .Include(b => b.Company)
            .ToListAsync();
        
        return Ok(ApiResponse<List<Branch>>.Success(branches));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDto dto)
    {
        var branch = new Branch
        {
            Code = dto.Code,
            Name = dto.Name,
            Address = dto.Address,
            CompanyId = dto.CompanyId
        };

        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetBranch), new { id = branch.Id }, ApiResponse<Branch>.Success(branch));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranch(Guid id, [FromBody] UpdateBranchDto dto)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            return NotFound(ApiResponse<Branch>.Error("Branch not found"));

        if (!string.IsNullOrEmpty(dto.Code)) branch.Code = dto.Code;
        if (!string.IsNullOrEmpty(dto.Name)) branch.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Address)) branch.Address = dto.Address;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Branch>.Success(branch));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(Guid id)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            return NotFound(ApiResponse<Branch>.Error("Branch not found"));

        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Branch deleted successfully"));
    }
}

public class CreateBranchDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
}

public class UpdateBranchDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}
