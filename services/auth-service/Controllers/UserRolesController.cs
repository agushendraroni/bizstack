using AuthService.DTOs.UserRole;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _service;

        public UserRolesController(IUserRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserRoleFilterRequest filter)
        {
            var result = await _service.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRoleRequest request)
        {
            var result = await _service.CreateAsync(request);
            // Gunakan composite key pada route values
            return CreatedAtAction(
                nameof(GetByCompositeKey),
                new { userId = result.UserId, roleId = result.RoleId },
                result
            );
        }

        // Tambahkan endpoint untuk GetByCompositeKey
        [HttpGet("by-key")]
        public async Task<IActionResult> GetByCompositeKey([FromQuery] int userId, [FromQuery] int roleId)
        {
            var result = await _service.GetByCompositeKeyAsync(userId, roleId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserRoleRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
