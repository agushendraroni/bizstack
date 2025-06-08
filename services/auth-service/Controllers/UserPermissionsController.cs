
using AuthService.DTOs.Common;
using AuthService.DTOs.UserPermission;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUserPermissionService _service;

        public UserPermissionController(IUserPermissionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserPermissionRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<UserPermissionResponse>.SuccessResponse(result));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserPermissionRequest request)
        {
            var result = await _service.UpdateAsync(request);
            return Ok(ApiResponse<UserPermissionResponse>.SuccessResponse(result));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int userId, [FromQuery] int permissionId)
        {
            var success = await _service.DeleteAsync(userId, permissionId);
            return Ok(ApiResponse<string>.SuccessResponse(success ? "Deleted" : "Not Found"));
        }

        [HttpGet("{userId}/{permissionId}")]
        public async Task<IActionResult> GetById(int userId, int permissionId)
        {
            var result = await _service.GetByIdAsync(userId, permissionId);
            return Ok(ApiResponse<UserPermissionResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserPermissionFilterRequest filter)
        {
            var result = await _service.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<UserPermissionResponse>>.SuccessResponse(result));
        }
    }
}
