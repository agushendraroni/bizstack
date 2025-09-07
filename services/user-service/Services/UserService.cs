using Microsoft.EntityFrameworkCore;
using AutoMapper;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;
using SharedLibrary.DTOs;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync(int? tenantId = null)
    {
        try
        {
            var query = _context.Users.Where(u => !u.IsDeleted).AsQueryable();
            if (tenantId.HasValue)
                query = query.Where(u => u.TenantId == tenantId.Value);
                
            var users = await query.ToListAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return ApiResponse<IEnumerable<UserDto>>.Success(userDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserDto>>.Error($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return ApiResponse<UserDto>.Error("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Error($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetUserByUsernameAsync(string username)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return ApiResponse<UserDto>.Error("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Error($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto, int? tenantId = null, Guid? userId = null)
    {
        try
        {
            var user = _mapper.Map<User>(createUserDto);
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.TenantId = tenantId;
            user.CreatedBy = userId?.ToString();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Error($"Error creating user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto, int? tenantId = null, Guid? userId = null)
    {
        try
        {
            var query = _context.Users.Where(x => !x.IsDeleted).AsQueryable();
            if (tenantId.HasValue)
                query = query.Where(u => u.TenantId == tenantId.Value);
                
            var user = await query.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return ApiResponse<UserDto>.Error("User not found");

            _mapper.Map(updateUserDto, user);
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userId?.ToString();

            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Error($"Error updating user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(Guid id, int? tenantId = null, Guid? userId = null)
    {
        try
        {
            var query = _context.Users.Where(x => !x.IsDeleted).AsQueryable();
            if (tenantId.HasValue)
                query = query.Where(u => u.TenantId == tenantId.Value);
                
            var user = await query.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return ApiResponse<bool>.Error("User not found");

            // Soft delete with audit trail
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userId?.ToString();
            
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Error($"Error deleting user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetUsersByOrganizationAsync(Guid organizationId)
    {
        try
        {
            var users = await _context.Users.Where(x => !x.IsDeleted)
                .Where(u => u.OrganizationId == organizationId)
                .ToListAsync();

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return ApiResponse<IEnumerable<UserDto>>.Success(userDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserDto>>.Error($"Error retrieving users: {ex.Message}");
        }
    }
}
