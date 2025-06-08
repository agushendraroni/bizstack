using AuthService.Data;
using AuthService.DTOs.UserPasswordHistory;
using AuthService.Models;
using AuthService.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class UserPasswordHistoryService : IUserPasswordHistoryService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserPasswordHistoryRequest> _createValidator;
        private readonly IValidator<UpdateUserPasswordHistoryRequest> _updateValidator;

        public UserPasswordHistoryService(
            AuthDbContext context,
            IMapper mapper,
            IValidator<CreateUserPasswordHistoryRequest> createValidator,
            IValidator<UpdateUserPasswordHistoryRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<UserPasswordHistoryResponse> CreateAsync(CreateUserPasswordHistoryRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var history = _mapper.Map<UserPasswordHistory>(request);
            _context.UserPasswordHistories.Add(history);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserPasswordHistoryResponse>(history);
        }

        public async Task<UserPasswordHistoryResponse> UpdateAsync(int id, UpdateUserPasswordHistoryRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

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