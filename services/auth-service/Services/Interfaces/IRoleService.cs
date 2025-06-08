using SharedLibrary.DTOs;
using AuthService.DTOs.Role;

namespace AuthService.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponse?> GetByIdAsync(int id);
        Task<PaginatedResponse<RoleResponse>> GetAllAsync(RoleFilterRequest filter);
        Task<RoleResponse> CreateAsync(CreateRoleRequest request, string createdBy);
        Task<RoleResponse?> UpdateAsync(int id, UpdateRoleRequest request, string updatedBy);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}
