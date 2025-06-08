using AuthService.DTOs.User;
using AuthService.DTOs.Common;

namespace AuthService.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteAsync(int id);
        Task<UserResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<UserResponse>> GetAllAsync(UserFilterRequest filter);
    }
}
