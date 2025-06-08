using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AuthService.Data;
using AuthService.Models;
using AuthService.DTOs.UserRole;
using AuthService.Services.Interfaces;
using AuthService.DTOs.Common;

namespace AuthService.Services.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public UserRoleService(AuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request)
        {
            var exists = await _context.UserRoles.AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId && !ur.IsDeleted);
            if (exists)
                throw new InvalidOperationException("UserRole already exists.");

            var entity = _mapper.Map<UserRole>(request);
            _context.UserRoles.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserRoleResponse>(entity);
        }

        public async Task<UserRoleResponse> UpdateAsync(int userId, int roleId, UpdateUserRoleRequest request)
        {
            var entity = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && !ur.IsDeleted);

            if (entity is null)
                throw new KeyNotFoundException("UserRole not found.");

            // Update properties, if user wants to change UserId or RoleId, 
            // better handle carefully, here kita anggap gak boleh update composite key
            // Jadi hanya boleh update non-key fields, tapi di model ini gak ada

            // Misal kita cuma update IsActive, etc, tapi karena gak ada, skip

            entity.ChangedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserRoleResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int userId, int roleId)
        {
            var entity = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && !ur.IsDeleted);

            if (entity is null)
                throw new KeyNotFoundException("UserRole not found.");

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserRoleResponse> GetByIdAsync(int userId, int roleId)
        {
            var entity = await _context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && !ur.IsDeleted);

            if (entity is null)
                throw new KeyNotFoundException("UserRole not found.");

            return _mapper.Map<UserRoleResponse>(entity);
        }

        public async Task<PaginatedResponse<UserRoleResponse>> GetAllAsync(UserRoleFilterRequest filter)
        {
            var query = _context.UserRoles.AsQueryable().Where(x => !x.IsDeleted);

            if (filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId.Value);

            if (filter.RoleId.HasValue)
                query = query.Where(x => x.RoleId == filter.RoleId.Value);

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.UserId).ThenBy(x => x.RoleId)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<UserRoleResponse>>(data);
            return new PaginatedResponse<UserRoleResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}