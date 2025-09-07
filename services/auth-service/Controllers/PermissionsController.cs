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
public class PermissionsController : ControllerBase
{
    private readonly AuthDbContext _context;

    public PermissionsController(AuthDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = await _context.Permissions.ToListAsync();
        return Ok(new ApiResponse<List<Permission>> { Data = permissions, IsSuccess = true, Message = "Permissions retrieved successfully" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPermission(Guid id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return NotFound(new ApiResponse<Permission> { Data = null, IsSuccess = false, Message = "Permission not found" });
        
        return Ok(new ApiResponse<Permission> { Data = permission, IsSuccess = true, Message = "Permission retrieved successfully" });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto dto)
    {
        var permission = new Permission
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, new ApiResponse<Permission> { Data = permission, IsSuccess = true, Message = "Permission created successfully" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionDto dto)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return NotFound(new ApiResponse<Permission> { Data = null, IsSuccess = false, Message = "Permission not found" });

        if (!string.IsNullOrEmpty(dto.Name)) permission.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Description)) permission.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(new ApiResponse<Permission> { Data = permission, IsSuccess = true, Message = "Permission updated successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(Guid id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return NotFound(new ApiResponse<Permission> { Data = null, IsSuccess = false, Message = "Permission not found" });

        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Permission deleted successfully", IsSuccess = true, Message = "Permission deleted successfully" });
    }

    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetRolePermissions(Guid roleId)
    {
        var permissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
        
        return Ok(new ApiResponse<List<Permission>> { Data = permissions, IsSuccess = true, Message = "Role permissions retrieved successfully" });
    }

    [HttpPost("role/{roleId}/assign/{permissionId}")]
    public async Task<IActionResult> AssignPermissionToRole(Guid roleId, Guid permissionId)
    {
        var exists = await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        
        if (exists)
            return BadRequest(new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Permission already assigned to role" });

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        _context.RolePermissions.Add(rolePermission);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Permission assigned to role successfully", IsSuccess = true, Message = "Permission assigned to role successfully" });
    }

    [HttpDelete("role/{roleId}/remove/{permissionId}")]
    public async Task<IActionResult> RemovePermissionFromRole(Guid roleId, Guid permissionId)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        
        if (rolePermission == null)
            return NotFound(new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Permission not assigned to role" });

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Permission removed from role successfully", IsSuccess = true, Message = "Permission removed from role successfully" });
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

public class CreatePermissionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdatePermissionDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
