using AuthService.Data;
using AuthService.DTOs.UserPasswordHistory;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class UserPasswordHistoryService : IUserPasswordHistoryService
    {
        private readonly AuthDbContext _context;

        public UserPasswordHistoryService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserPasswordHistoryResponse> CreateAsync(CreateUserPasswordHistoryRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) throw new InvalidOperationException("User not found");

            var entity = new UserPasswordHistory
            {
                UserId = request.UserId,
                PasswordHash = request.PasswordHash,
                User = user
            };

            _context.UserPasswordHistories.Add(entity);
            await _context.SaveChangesAsync();

            return MapToResponse(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.UserPasswordHistories.FindAsync(id);
            if (entity == null) return false;

            _context.UserPasswordHistories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserPasswordHistoryResponse>> GetAllAsync(UserPasswordHistoryFilterRequest filter)
        {
            var query = _context.UserPasswordHistories.AsQueryable();

            if (filter?.UserId.HasValue == true)
                query = query.Where(x => x.UserId == filter.UserId);

            var entities = await query.ToListAsync();
            return entities.Select(MapToResponse);
        }

        public async Task<UserPasswordHistoryResponse?> GetByIdAsync(int id)
        {
            var entity = await _context.UserPasswordHistories.FindAsync(id);
            return entity == null ? null : MapToResponse(entity);
        }

        public async Task<UserPasswordHistoryResponse?> UpdateAsync(int id, UpdateUserPasswordHistoryRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var entity = await _context.UserPasswordHistories.FindAsync(id);
            if (entity == null) return null;

            entity.PasswordHash = request.PasswordHash;
            await _context.SaveChangesAsync();

            return MapToResponse(entity);
        }

        private static UserPasswordHistoryResponse MapToResponse(UserPasswordHistory entity) => new()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            PasswordHash = entity.PasswordHash,
            CreatedAt = entity.CreatedAt
        };
    }
}