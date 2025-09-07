using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using AuthService.Data;
using AuthService.Models;
using SharedLibrary.DTOs;

namespace AuthService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AuthDbContext _context;

    public RolesController(AuthDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var tenantId = GetTenantId();
        var query = _context.Roles.AsQueryable();
        
        if (tenantId.HasValue)
            query = query.Where(r => r.TenantId == tenantId.Value);
            
        var roles = await query.ToListAsync();
        return Ok(new ApiResponse<List<Role>> { Data = roles, IsSuccess = true });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(new ApiResponse<Role> { Data = null, IsSuccess = false, Message = "Role not found" });
        
        return Ok(new ApiResponse<Role> { Data = role, IsSuccess = true });
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
    {
        var tenantId = GetTenantId();
        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            TenantId = tenantId
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, new ApiResponse<Role> { Data = role, IsSuccess = true });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto dto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(new ApiResponse<Role> { Data = null, IsSuccess = false, Message = "Role not found" });

        if (!string.IsNullOrEmpty(dto.Name)) role.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Description)) role.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(new ApiResponse<Role> { Data = role, IsSuccess = true });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(new ApiResponse<Role> { Data = null, IsSuccess = false, Message = "Role not found" });

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Role deleted successfully", IsSuccess = true });
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

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateRoleDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}