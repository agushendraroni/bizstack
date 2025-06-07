using AuthService.DTOs.UserPermission;

namespace AuthService.Services.Interfaces
{
    public interface IUserPermissionService
    {
        Task<IEnumerable<UserPermissionResponse>> GetAllAsync(UserPermissionFilterRequest filter);
        Task<UserPermissionResponse?> GetByIdAsync(int id);
        Task<UserPermissionResponse> CreateAsync(CreateUserPermissionRequest request);
        Task<UserPermissionResponse?> UpdateAsync(int id, UpdateUserPermissionRequest request);
        Task<bool> DeleteAsync(int id);
    }
}