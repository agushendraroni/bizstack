using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Data;
using AuthService.Models;
using AuthService.DTOs.Role;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly AuthDbContext _context;

    public RoleService(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<RoleResponse> CreateAsync(CreateRoleRequest request, string currentUser)
    {
        var role = new Role
        {
            Name = request.Name,
            CompanyId = request.CompanyId,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUser
        };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return MapToResponse(role);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return false;
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(RoleFilterRequest filter)
    {
        var query = _context.Roles.AsQueryable();

        if (filter.CompanyId.HasValue)
            query = query.Where(r => r.CompanyId == filter.CompanyId.Value);

        if (filter.IsActive.HasValue)
            query = query.Where(r => r.IsActive == filter.IsActive.Value);

        if (!string.IsNullOrEmpty(filter.NameContains))
            query = query.Where(r => r.Name.Contains(filter.NameContains));

        int skip = ((filter.Page ?? 1) - 1) * (filter.PageSize ?? 10);
        query = query.Skip(skip).Take(filter.PageSize ?? 10);

        var roles = await query.ToListAsync();
        return roles.Select(MapToResponse);
    }

    public async Task<RoleResponse?> GetByIdAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return null;
        return MapToResponse(role);
    }

    public async Task<RoleResponse?> UpdateAsync(int id, UpdateRoleRequest request, string currentUser)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return null;
        role.Name = request.Name;
        role.IsActive = request.IsActive;
        role.ChangedAt = DateTime.UtcNow;
        role.ChangedBy = currentUser;
        await _context.SaveChangesAsync();
        return MapToResponse(role);
    }

    private RoleResponse MapToResponse(Role role)
    {
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            CompanyId = role.CompanyId,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            CreatedBy = role.CreatedBy,
            ChangedAt = role.ChangedAt,
            ChangedBy = role.ChangedBy
        };
    }
}
