using AutoMapper;
using AuthService.DTOs;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Helpers;
using AuthService.DTOs.RolePermission;
using SharedLibrary.DTOs;
using FluentValidation;

namespace AuthService.Services.Implementations
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRolePermissionRequest> _createValidator;
        private readonly IValidator<UpdateRolePermissionRequest> _updateValidator;

        public RolePermissionService(
            AuthDbContext context,
            IMapper mapper,
            IValidator<CreateRolePermissionRequest> createValidator,
            IValidator<UpdateRolePermissionRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<RolePermissionResponse> CreateAsync(CreateRolePermissionRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<RolePermission>(request);
            _context.RolePermissions.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<RolePermissionResponse>(entity);
        }

        public async Task<RolePermissionResponse> UpdateAsync(UpdateRolePermissionRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var entity = await _context.RolePermissions
                .FirstOrDefaultAsync(x => x.RoleId == request.RoleId && x.PermissionId == request.PermissionId);

            if (entity == null)
                throw new KeyNotFoundException("RolePermission not found");

            // No fields to update currently since composite key is used, add fields if needed

            _context.RolePermissions.Update(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<RolePermissionResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int roleId, int permissionId)
        {
            var entity = await _context.RolePermissions
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId);

            if (entity == null) return false;

            _context.RolePermissions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RolePermissionResponse>> GetAllAsync()
        {
            var entities = await _context.RolePermissions.ToListAsync();
            return _mapper.Map<IEnumerable<RolePermissionResponse>>(entities);
        }

        public async Task<PaginatedResponse<RolePermissionResponse>> GetPagedAsync(RolePermissionFilterRequest filter)
        {
            var query = _context.RolePermissions.AsQueryable();

            if (filter.RoleId.HasValue)
                query = query.Where(x => x.RoleId == filter.RoleId);
            if (filter.PermissionId.HasValue)
                query = query.Where(x => x.PermissionId == filter.PermissionId);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<RolePermissionResponse>>(items);

            return new PaginatedResponse<RolePermissionResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}