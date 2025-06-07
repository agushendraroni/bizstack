using AuthService.Data;
using AuthService.DTOs.UserPermission;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly AuthDbContext _context;

        public UserPermissionService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserPermissionResponse> CreateAsync(CreateUserPermissionRequest request)
        {
            var entity = new UserPermission
            {
                UserId = request.UserId,
                PermissionId = request.PermissionId
            };

            _context.UserPermissions.Add(entity);
            await _context.SaveChangesAsync();

            return MapToResponse(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.UserPermissions.FindAsync(id);
            if (entity == null) return false;

            _context.UserPermissions.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserPermissionResponse>> GetAllAsync(UserPermissionFilterRequest filter)
        {
            var query = _context.UserPermissions.AsQueryable();

            if (filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter.PermissionId.HasValue)
                query = query.Where(x => x.PermissionId == filter.PermissionId);

            var result = await query.ToListAsync();
            return result.Select(MapToResponse);
        }

        public async Task<UserPermissionResponse?> GetByIdAsync(int id)
        {
            var entity = await _context.UserPermissions.FindAsync(id);
            return entity == null ? null : MapToResponse(entity);
        }

        public async Task<UserPermissionResponse?> UpdateAsync(int id, UpdateUserPermissionRequest request)
        {
            var entity = await _context.UserPermissions.FindAsync(id);
            if (entity == null) return null;

            entity.PermissionId = request.PermissionId;
            await _context.SaveChangesAsync();

            return MapToResponse(entity);
        }

        private UserPermissionResponse MapToResponse(UserPermission entity) => new()
        {
            UserId = entity.UserId,
            PermissionId = entity.PermissionId
        };
    }
}