using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services;
using AuthService.Models;
using AuthService.Services.Interfaces;
using AuthService.DTOs.Common;
using AuthService.DTOs.UserRole;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _service;

        public UserRolesController(IUserRoleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRoleRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<UserRoleResponse>.SuccessResponse(result));
        }

        // Pakai userId dan roleId di route sebagai composite key
        [HttpPut("{userId:int}/{roleId:int}")]
        public async Task<IActionResult> Update(int userId, int roleId, [FromBody] UpdateUserRoleRequest request)
        {
            var result = await _service.UpdateAsync(userId, roleId, request);
            return Ok(ApiResponse<UserRoleResponse>.SuccessResponse(result));
        }

        [HttpDelete("{userId:int}/{roleId:int}")]
        public async Task<IActionResult> Delete(int userId, int roleId)
        {
            await _service.DeleteAsync(userId, roleId);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{userId:int}/{roleId:int}")]
        public async Task<IActionResult> GetById(int userId, int roleId)
        {
            var result = await _service.GetByIdAsync(userId, roleId);
            return Ok(ApiResponse<UserRoleResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserRoleFilterRequest filter)
        {
            var result = await _service.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<UserRoleResponse>>.SuccessResponse(result));
        }
    }
}