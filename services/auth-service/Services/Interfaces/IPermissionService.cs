using SharedLibrary.DTOs;
using AuthService.DTOs.Permission;

namespace AuthService.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionResponse> CreateAsync(CreatePermissionRequest request, string createdBy);
        Task<PermissionResponse> UpdateAsync(int id, UpdatePermissionRequest request, string changedBy);
        Task<bool> DeleteAsync(int id, string changedBy);
        Task<PaginatedResponse<PermissionResponse>> GetAllAsync(PermissionFilterRequest filter);
        Task<PermissionResponse?> GetByIdAsync(int id);
    }
}