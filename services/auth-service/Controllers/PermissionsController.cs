
using AuthService.DTOs.Common;
using AuthService.DTOs.Permission;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PermissionResponse>>> Create([FromBody] CreatePermissionRequest request)
        {
            var result = await _service.CreateAsync(request, User.Identity?.Name ?? "system");
            return Ok(ApiResponse<PermissionResponse>.SuccessResponse(result));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<PermissionResponse>>> Update(int id, [FromBody] UpdatePermissionRequest request)
        {
            var result = await _service.UpdateAsync(id, request, User.Identity?.Name ?? "system");
            return Ok(ApiResponse<PermissionResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var success = await _service.DeleteAsync(id, User.Identity?.Name ?? "system");
            if (!success)
                return NotFound(ApiResponse<string>.Fail("Permission not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<PermissionResponse>>>> GetAll([FromQuery] PermissionFilterRequest filter)
        {
            var result = await _service.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<PermissionResponse>>.SuccessResponse(result));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<PermissionResponse>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<PermissionResponse>.Fail("Permission not found"));

            return Ok(ApiResponse<PermissionResponse>.SuccessResponse(result));
        }
    }
}