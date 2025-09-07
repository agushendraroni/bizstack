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
public class JobTitlesController : ControllerBase
{
    private readonly OrganizationDbContext _context;

    public JobTitlesController(OrganizationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobTitles()
    {
        var tenantId = GetTenantId();
        var query = _context.JobTitles.AsQueryable();
        
        if (tenantId.HasValue)
            query = query.Where(jt => jt.TenantId == tenantId.Value);
            
        var jobTitles = await query.ToListAsync();
        return Ok(ApiResponse<List<JobTitle>>.Success(jobTitles));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobTitle(Guid id)
    {
        var jobTitle = await _context.JobTitles.FindAsync(id);
        if (jobTitle == null)
            return NotFound(ApiResponse<JobTitle>.Error("Job title not found"));
        
        return Ok(ApiResponse<JobTitle>.Success(jobTitle));
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobTitle([FromBody] CreateJobTitleDto dto)
    {
        var tenantId = GetTenantId();
        var jobTitle = new JobTitle
        {
            Title = dto.Title,
            Description = dto.Description,
            TenantId = tenantId
        };

        _context.JobTitles.Add(jobTitle);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetJobTitle), new { id = jobTitle.Id }, ApiResponse<JobTitle>.Success(jobTitle));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJobTitle(Guid id, [FromBody] UpdateJobTitleDto dto)
    {
        var jobTitle = await _context.JobTitles.FindAsync(id);
        if (jobTitle == null)
            return NotFound(ApiResponse<JobTitle>.Error("Job title not found"));

        if (!string.IsNullOrEmpty(dto.Title)) jobTitle.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.Description)) jobTitle.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<JobTitle>.Success(jobTitle));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobTitle(Guid id)
    {
        var jobTitle = await _context.JobTitles.FindAsync(id);
        if (jobTitle == null)
            return NotFound(ApiResponse<JobTitle>.Error("Job title not found"));

        _context.JobTitles.Remove(jobTitle);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Job title deleted successfully"));
    }

    [HttpGet("search/{title}")]
    public async Task<IActionResult> SearchJobTitles(string title)
    {
        var jobTitles = await _context.JobTitles
            .Where(jt => jt.Title.Contains(title))
            .ToListAsync();
        
        return Ok(ApiResponse<List<JobTitle>>.Success(jobTitles));
    }

    private int? GetTenantId()
    {
        return null;
    }
}

public class CreateJobTitleDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateJobTitleDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }

    private Guid? GetUserId()
    {
        return null;
    }
}
