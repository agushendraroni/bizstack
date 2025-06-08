using AuthService.Data;
using AuthService.DTOs.User;
using SharedLibrary.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace AuthService.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserRequest> _createValidator;
        private readonly IValidator<UpdateUserRequest> _updateValidator;

        public UserService(
            AuthDbContext context,
            IMapper mapper,
            IValidator<CreateUserRequest> createValidator,
            IValidator<UpdateUserRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var user = await _context.Users.FindAsync(id)
                ?? throw new KeyNotFoundException("User not found");

            _mapper.Map(request, user);
            if (!string.IsNullOrWhiteSpace(request.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id)
                ?? throw new KeyNotFoundException("User not found");

            user.IsDeleted = true;
            user.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
                ?? throw new KeyNotFoundException("User not found");

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<PaginatedResponse<UserResponse>> GetAllAsync(UserFilterRequest filter)
        {
            var query = _context.Users.AsQueryable().Where(u => !u.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Username))
                query = query.Where(u => u.Username.Contains(filter.Username));

            if (filter.CompanyId.HasValue)
                query = query.Where(u => u.CompanyId == filter.CompanyId);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(u => u.Username)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<UserResponse>>(data);
            return new PaginatedResponse<UserResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}