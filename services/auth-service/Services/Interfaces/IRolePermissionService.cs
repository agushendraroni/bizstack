using AuthService.DTOs.Common;
using AuthService.DTOs.RolePermission;

namespace AuthService.Services.Interfaces
{
    public interface IRolePermissionService
    {
        Task<RolePermissionResponse> CreateAsync(CreateRolePermissionRequest request);
        Task<RolePermissionResponse> UpdateAsync(UpdateRolePermissionRequest request);
        Task<bool> DeleteAsync(int roleId, int permissionId);
        Task<IEnumerable<RolePermissionResponse>> GetAllAsync();
        Task<PaginatedResponse<RolePermissionResponse>> GetPagedAsync(RolePermissionFilterRequest filter);
    }
}