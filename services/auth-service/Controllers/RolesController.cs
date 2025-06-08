using AuthService.DTOs.Common;
using AuthService.DTOs.Role;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        private string GetUser() =>
            User.FindFirstValue(ClaimTypes.Name) ?? "system";

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return role == null
                ? NotFound(ApiResponse<string>.Fail("Role not found"))
                : Ok(ApiResponse<RoleResponse>.SuccessResponse(role));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RoleFilterRequest filter)
        {
            var result = await _roleService.GetAllAsync(filter);
            return Ok(ApiResponse<IEnumerable<RoleResponse>>.SuccessResponse(result.Data, meta: new { result.TotalCount, result.Page, result.PageSize }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            var result = await _roleService.CreateAsync(request, GetUser());
            return Ok(ApiResponse<RoleResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleRequest request)
        {
            var result = await _roleService.UpdateAsync(id, request, GetUser());
            return result == null
                ? NotFound(ApiResponse<string>.Fail("Role not found"))
                : Ok(ApiResponse<RoleResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _roleService.DeleteAsync(id, GetUser());
            return success
                ? Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"))
                : NotFound(ApiResponse<string>.Fail("Role not found"));
        }
    }
}
