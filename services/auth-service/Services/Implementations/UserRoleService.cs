using AuthService.Data;
using AuthService.DTOs.UserRole;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AuthDbContext _context;

        public UserRoleService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request)
        {
            var userRole = new UserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return MapToResponse(userRole);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null) return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserRoleResponse>> GetAllAsync(UserRoleFilterRequest filter)
        {
            var query = _context.UserRoles.AsQueryable();

            if (filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter.RoleId.HasValue)
                query = query.Where(x => x.RoleId == filter.RoleId);

            var result = await query.ToListAsync();
            return result.Select(MapToResponse);
        }

        public async Task<UserRoleResponse> GetByCompositeKeyAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
                throw new InvalidOperationException("UserRole not found.");

            return MapToResponse(userRole);
        }

        public async Task<UserRoleResponse?> GetByIdAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            return userRole == null ? null : MapToResponse(userRole);
        }

        public async Task<UserRoleResponse?> UpdateAsync(int id, UpdateUserRoleRequest request)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null) return null;

            userRole.RoleId = request.RoleId;
            await _context.SaveChangesAsync();

            return MapToResponse(userRole);
        }

        private UserRoleResponse MapToResponse(UserRole entity) => new()
        {
            UserId = entity.UserId,
            RoleId = entity.RoleId
        };
    }
}