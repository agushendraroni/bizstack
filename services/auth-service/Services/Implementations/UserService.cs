// Services/Implementations/UserService.cs
using AuthService.Data;
using AuthService.DTOs.User;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    /// <summary>
    /// Implementasi service user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AuthDbContext _context;

        public UserService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request, string createdBy = "system")
        {
            var entity = new User
            {
                Username = request.Username,
                CompanyId = request.CompanyId,
                PasswordHash = request.PasswordHash,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsDeleted = false
            };

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return MapToResponse(entity);
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy = "system")
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null || entity.IsDeleted) return false;

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            entity.ChangedBy = deletedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync(UserFilterRequest filter)
        {
            var query = _context.Users
                .Include(u => u.Company)
                .Where(u => !u.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Username))
                query = query.Where(u => u.Username.Contains(filter.Username));

            if (filter.CompanyId.HasValue)
                query = query.Where(u => u.CompanyId == filter.CompanyId.Value);

            // Sorting
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }
            else
            {
                query = query.OrderBy(u => u.Id);
            }

            // Pagination
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            var list = await query.ToListAsync();
            return list.Select(MapToResponse);
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var entity = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            return entity == null ? null : MapToResponse(entity);
        }

        public async Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request, string updatedBy = "system")
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            entity.Username = request.Username;
            entity.CompanyId = request.CompanyId;
            entity.ChangedAt = DateTime.UtcNow;
            entity.ChangedBy = updatedBy;

            await _context.SaveChangesAsync();

            return MapToResponse(entity);
        }

        private static UserResponse MapToResponse(User entity) => new()
        {
            Id = entity.Id,
            Username = entity.Username,
            CompanyId = entity.CompanyId,
            CompanyName = entity.Company?.Name ?? string.Empty,
            LoginFailCount = entity.LoginFailCount,
            LastLoginAt = entity.LastLoginAt,
            LastFailedLoginAt = entity.LastFailedLoginAt,
            CreatedAt = entity.CreatedAt,
            ChangedAt = entity.ChangedAt,
            CreatedBy = entity.CreatedBy,
            ChangedBy = entity.ChangedBy,
        };
    }
}
