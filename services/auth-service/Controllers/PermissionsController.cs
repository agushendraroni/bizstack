using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Models;
using SharedLibrary.DTOs;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return Ok(ApiResponse<List<Permission>>.Success(permissions));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPermission(Guid id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return NotFound(ApiResponse<Permission>.Error("Permission not found"));
        
        return Ok(ApiResponse<Permission>.Success(permission));
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
        
        return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, ApiResponse<Permission>.Success(permission));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionDto dto)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return NotFound(ApiResponse<Permission>.Error("Permission not found"));

        if (!string.IsNullOrEmpty(dto.Name)) permission.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Description)) permission.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Permission>.Success(permission));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(Guid id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return NotFound(ApiResponse<Permission>.Error("Permission not found"));

        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Permission deleted successfully"));
    }

    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetRolePermissions(Guid roleId)
    {
        var permissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
        
        return Ok(ApiResponse<List<Permission>>.Success(permissions));
    }

    [HttpPost("role/{roleId}/assign/{permissionId}")]
    public async Task<IActionResult> AssignPermissionToRole(Guid roleId, Guid permissionId)
    {
        var exists = await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        
        if (exists)
            return BadRequest(ApiResponse<string>.Error("Permission already assigned to role"));

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        _context.RolePermissions.Add(rolePermission);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Permission assigned to role successfully"));
    }

    [HttpDelete("role/{roleId}/remove/{permissionId}")]
    public async Task<IActionResult> RemovePermissionFromRole(Guid roleId, Guid permissionId)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        
        if (rolePermission == null)
            return NotFound(ApiResponse<string>.Error("Permission not assigned to role"));

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Permission removed from role successfully"));
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
