using Microsoft.AspNetCore.Mvc;
using AuthService.Services.Interfaces;
using AuthService.DTOs;
using AuthService.Helpers;
using AuthService.DTOs.RolePermission;
using SharedLibrary.DTOs;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionService _service;

        public RolePermissionsController(IRolePermissionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRolePermissionRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<RolePermissionResponse>.SuccessResponse(result));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRolePermissionRequest request)
        {
            var result = await _service.UpdateAsync(request);
            return Ok(ApiResponse<RolePermissionResponse>.SuccessResponse(result));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int roleId, [FromQuery] int permissionId)
        {
            var result = await _service.DeleteAsync(roleId, permissionId);
            return Ok(ApiResponse<string>.SuccessResponse(result ? "Deleted" : "Not Found"));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] RolePermissionFilterRequest filter)
        {
            var result = await _service.GetPagedAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<RolePermissionResponse>>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<RolePermissionResponse>>.SuccessResponse(result));
        }
    }
}
