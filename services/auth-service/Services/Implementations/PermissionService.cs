using AuthService.Data;
using AuthService.DTOs.Permission;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly AuthDbContext _context;

        public PermissionService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<PermissionResponse> CreateAsync(CreatePermissionRequest request)
        {
            var permission = new Permission
            {
                Name = request.Name,
                Description = request.Description,
                CompanyId = request.CompanyId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description ?? string.Empty,
                CompanyId = permission.CompanyId
            };
        }

        public async Task<PermissionResponse?> GetByIdAsync(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return null;

            return new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description ?? string.Empty,
                CompanyId = permission.CompanyId
            };
        }

        public async Task<IEnumerable<PermissionResponse>> GetAllAsync(PermissionFilterRequest filter)
        {
            var query = _context.Permissions.AsQueryable();

            if (filter.CompanyId.HasValue)
                query = query.Where(p => p.CompanyId == filter.CompanyId);
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(p => p.Name.Contains(filter.Name));

            return await query
                .Select(p => new PermissionResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description ?? string.Empty,
                    CompanyId = p.CompanyId
                }).ToListAsync();
        }

        public async Task<PermissionResponse> UpdateAsync(int id, UpdatePermissionRequest request)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) throw new KeyNotFoundException("Permission not found");

            permission.Name = request.Name ?? permission.Name;
            permission.Description = request.Description ?? permission.Description;
            permission.ChangedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description ?? string.Empty,
                CompanyId = permission.CompanyId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return false;

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}