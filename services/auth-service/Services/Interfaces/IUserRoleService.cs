// ==========================
// Service Interface (IUserRoleService.cs)
using AuthService.DTOs.Common;
using AuthService.DTOs.UserRole;
namespace AuthService.Services.Interfaces;
public interface IUserRoleService
{
    Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request);
    Task<UserRoleResponse> UpdateAsync(int userId, int roleId, UpdateUserRoleRequest request);
    Task<bool> DeleteAsync(int userId, int roleId);
    Task<UserRoleResponse> GetByIdAsync(int userId, int roleId);
    Task<PaginatedResponse<UserRoleResponse>> GetAllAsync(UserRoleFilterRequest filter);
}
