using AuthService.DTOs.User;

namespace AuthService.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync(UserFilterRequest filter);
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(CreateUserRequest request, string createdBy = "system");
        Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request, string updatedBy = "system");
        Task<bool> DeleteAsync(int id, string deletedBy = "system");
    }
}
