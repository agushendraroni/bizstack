using AuthService.DTOs.Permission;

namespace AuthService.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionResponse> CreateAsync(CreatePermissionRequest request);
        Task<PermissionResponse> UpdateAsync(int id, UpdatePermissionRequest request);
        Task<bool> DeleteAsync(int id);
        Task<PermissionResponse?> GetByIdAsync(int id);
        Task<IEnumerable<PermissionResponse>> GetAllAsync(PermissionFilterRequest filter);
    }
}