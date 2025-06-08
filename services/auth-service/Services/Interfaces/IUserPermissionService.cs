using AuthService.DTOs.Common;
using AuthService.DTOs.UserPermission;

namespace AuthService.Services.Interfaces
{
     public interface IUserPermissionService
    {
        Task<UserPermissionResponse> CreateAsync(CreateUserPermissionRequest request);
        Task<UserPermissionResponse> UpdateAsync(UpdateUserPermissionRequest request);
        Task<bool> DeleteAsync(int userId, int permissionId);
        Task<UserPermissionResponse?> GetByIdAsync(int userId, int permissionId);
        Task<PaginatedResponse<UserPermissionResponse>> GetAllAsync(UserPermissionFilterRequest filter);
    }
}