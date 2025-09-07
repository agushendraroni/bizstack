using UserService.DTOs;
using SharedLibrary.DTOs;

namespace UserService.Services;

public interface IUserService
{
    Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync(int? tenantId = null);
    Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id);
    Task<ApiResponse<UserDto>> GetUserByUsernameAsync(string username);
    Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto, int? tenantId = null, Guid? userId = null);
    Task<ApiResponse<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto, int? tenantId = null, Guid? userId = null);
    Task<ApiResponse<bool>> DeleteUserAsync(Guid id, int? tenantId = null, Guid? userId = null);
    Task<ApiResponse<IEnumerable<UserDto>>> GetUsersByOrganizationAsync(Guid organizationId);
}
