using AuthService.DTOs.Menu;
using AuthService.DTOs.Common;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services.Interfaces;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMenuRequest request)
        {
            var result = await _menuService.CreateAsync(request);
            return Ok(ApiResponse<MenuResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMenuRequest request)
        {
            var result = await _menuService.UpdateAsync(id, request);
            return Ok(ApiResponse<MenuResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _menuService.GetByIdAsync(id);
            return Ok(ApiResponse<MenuResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MenuFilterRequest filter)
        {
            var result = await _menuService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<MenuResponse>>.SuccessResponse(result));
        }
    }
}
