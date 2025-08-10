using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.DTOs.Division;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DivisionsController : ControllerBase
    {
        private readonly IDivisionService _divisionService;

        public DivisionsController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDivisionRequest request)
        {
            var result = await _divisionService.CreateAsync(request);
            return Ok(ApiResponse<DivisionResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDivisionRequest request)
        {
            var result = await _divisionService.UpdateAsync(id, request);
            return Ok(ApiResponse<DivisionResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _divisionService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _divisionService.GetByIdAsync(id);
            return Ok(ApiResponse<DivisionResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DivisionFilterRequest filter)
        {
            var result = await _divisionService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<DivisionResponse>>.SuccessResponse(result));
        }
    }
}