using AuthService.DTOs.UserRole;

namespace AuthService.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleResponse>> GetAllAsync(UserRoleFilterRequest filter);
        Task<UserRoleResponse?> GetByIdAsync(int id);
        Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request);
        Task<UserRoleResponse?> UpdateAsync(int id, UpdateUserRoleRequest request);
        Task<bool> DeleteAsync(int id);
        Task<UserRoleResponse> GetByCompositeKeyAsync(int userId, int roleId);
    }
}