using AuthService.Data;
using AuthService.DTOs.UserPasswordHistory;
using AuthService.Models;
using AuthService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class UserPasswordHistoryService : IUserPasswordHistoryService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public UserPasswordHistoryService(AuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserPasswordHistoryResponse> CreateAsync(CreateUserPasswordHistoryRequest request)
        {
            var history = _mapper.Map<UserPasswordHistory>(request);
            _context.UserPasswordHistories.Add(history);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserPasswordHistoryResponse>(history);
        }

        public async Task<UserPasswordHistoryResponse> UpdateAsync(int id, UpdateUserPasswordHistoryRequest request)
        {
            var history = await _context.UserPasswordHistories.FindAsync(id);
            if (history == null) throw new KeyNotFoundException("User password history not found");

            _mapper.Map(request, history);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserPasswordHistoryResponse>(history);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var history = await _context.UserPasswordHistories.FindAsync(id);
            if (history == null) return false;

            _context.UserPasswordHistories.Remove(history);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserPasswordHistoryResponse?> GetByIdAsync(int id)
        {
            var history = await _context.UserPasswordHistories.FindAsync(id);
            return history == null ? null : _mapper.Map<UserPasswordHistoryResponse>(history);
        }

        public async Task<IEnumerable<UserPasswordHistoryResponse>> GetAllAsync()
        {
            var histories = await _context.UserPasswordHistories.ToListAsync();
            return _mapper.Map<IEnumerable<UserPasswordHistoryResponse>>(histories);
        }
    }
}