using AuthService.Data;
using AuthService.DTOs.Common;
using AuthService.DTOs.UserPermission;
using AuthService.Models;
using AuthService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public UserPermissionService(AuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserPermissionResponse> CreateAsync(CreateUserPermissionRequest request)
        {
            var entity = _mapper.Map<UserPermission>(request);
            _context.UserPermissions.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserPermissionResponse>(entity);
        }

        public async Task<UserPermissionResponse> UpdateAsync(UpdateUserPermissionRequest request)
        {
            var entity = await _context.UserPermissions
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.PermissionId == request.PermissionId);
            if (entity == null) throw new KeyNotFoundException("UserPermission not found");

            _mapper.Map(request, entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserPermissionResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int userId, int permissionId)
        {
            var entity = await _context.UserPermissions
                .FirstOrDefaultAsync(x => x.UserId == userId && x.PermissionId == permissionId);
            if (entity == null) return false;
            _context.UserPermissions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserPermissionResponse?> GetByIdAsync(int userId, int permissionId)
        {
            var entity = await _context.UserPermissions
                .FirstOrDefaultAsync(x => x.UserId == userId && x.PermissionId == permissionId);
            return entity != null ? _mapper.Map<UserPermissionResponse>(entity) : null;
        }

        public async Task<PaginatedResponse<UserPermissionResponse>> GetAllAsync(UserPermissionFilterRequest filter)
        {
            var query = _context.UserPermissions.AsQueryable();

            if (filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId);
            if (filter.PermissionId.HasValue)
                query = query.Where(x => x.PermissionId == filter.PermissionId);

            var total = await query.CountAsync();



             var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var mapped = _mapper.Map<List<UserPermissionResponse>>(items);
            return new PaginatedResponse<UserPermissionResponse>(mapped, total, filter.Page, filter.PageSize);
        }

        Task<PaginatedResponse<UserPermissionResponse>> IUserPermissionService.GetAllAsync(UserPermissionFilterRequest filter)
        {
            throw new NotImplementedException();
        }
    }
}