using AuthService.Data;
using AuthService.DTOs.Common;
using AuthService.DTOs.Permission;
using AuthService.Models;
using AuthService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public PermissionService(AuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PermissionResponse> CreateAsync(CreatePermissionRequest request, string createdBy)
        {
            var entity = _mapper.Map<Permission>(request);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            _context.Permissions.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<PermissionResponse>(entity);
        }

        public async Task<PermissionResponse> UpdateAsync(int id, UpdatePermissionRequest request, string changedBy)
        {
            var entity = await _context.Permissions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) throw new KeyNotFoundException("Permission not found");

            _mapper.Map(request, entity);
            entity.ChangedAt = DateTime.UtcNow;
            entity.ChangedBy = changedBy;

            await _context.SaveChangesAsync();
            return _mapper.Map<PermissionResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int id, string changedBy)
        {
            var entity = await _context.Permissions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            entity.ChangedBy = changedBy;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResponse<PermissionResponse>> GetAllAsync(PermissionFilterRequest filter)
        {
            var query = _context.Permissions.AsNoTracking().Where(x => !x.IsDeleted);

            if (filter.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == filter.CompanyId);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Name.Contains(filter.Search));

            if (filter.Type.HasValue)
                query = query.Where(x => x.Type == filter.Type);

            // Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "name" => filter.SortOrder == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                    "type" => filter.SortOrder == "desc" ? query.OrderByDescending(x => x.Type) : query.OrderBy(x => x.Type),
                    "createdat" => filter.SortOrder == "desc" ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                    _ => query.OrderBy(x => x.Id)
                };
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }

            var totalCount = await query.CountAsync();
            var data = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<PermissionResponse>>(data);

            return new PaginatedResponse<PermissionResponse>(mappedData, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<PermissionResponse?> GetByIdAsync(int id)
        {
            var entity = await _context.Permissions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return entity == null ? null : _mapper.Map<PermissionResponse>(entity);
        }
    }
}