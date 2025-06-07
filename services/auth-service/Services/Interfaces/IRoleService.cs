

using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.DTOs.Role;

namespace AuthService.Services.Interfaces;


public interface IRoleService
{
    Task<RoleResponse> CreateAsync(CreateRoleRequest request, string currentUser);
    Task<RoleResponse?> GetByIdAsync(int id);
    Task<IEnumerable<RoleResponse>> GetAllAsync(RoleFilterRequest filter);
    Task<RoleResponse?> UpdateAsync(int id, UpdateRoleRequest request, string currentUser);
    Task<bool> DeleteAsync(int id);
}
