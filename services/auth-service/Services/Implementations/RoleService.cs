using AuthService.Data;
using SharedLibrary.DTOs;
using AuthService.DTOs.Role;
using AuthService.Interfaces;
using AuthService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace AuthService.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRoleRequest> _createValidator;
        private readonly IValidator<UpdateRoleRequest> _updateValidator;

        public RoleService(
            AuthDbContext context,
            IMapper mapper,
            IValidator<CreateRoleRequest> createValidator,
            IValidator<UpdateRoleRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<RoleResponse?> GetByIdAsync(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            return role == null ? null : _mapper.Map<RoleResponse>(role);
        }

        public async Task<PaginatedResponse<RoleResponse>> GetAllAsync(RoleFilterRequest filter)
        {
            var query = _context.Roles
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(r => r.Name.Contains(filter.Name));

            if (filter.CompanyId.HasValue)
                query = query.Where(r => r.CompanyId == filter.CompanyId.Value);

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }

            var totalCount = await query.CountAsync();

            var roles = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var data = _mapper.Map<List<RoleResponse>>(roles);
            return new PaginatedResponse<RoleResponse>(data, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest request, string createdBy)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var role = _mapper.Map<Role>(request);
            role.CreatedAt = DateTime.UtcNow;
            role.CreatedBy = createdBy;

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse?> UpdateAsync(int id, UpdateRoleRequest request, string updatedBy)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (role == null) return null;

            role.Name = request.Name;
            role.CompanyId = request.CompanyId;
            role.ChangedAt = DateTime.UtcNow;
            role.ChangedBy = updatedBy;

            await _context.SaveChangesAsync();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (role == null) return false;

            role.IsDeleted = true;
            role.ChangedAt = DateTime.UtcNow;
            role.ChangedBy = deletedBy;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
