using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.DTOs.CostCenter;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CostCentersController : ControllerBase
    {
        private readonly ICostCenterService _costCenterService;

        public CostCentersController(ICostCenterService costCenterService)
        {
            _costCenterService = costCenterService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCostCenterRequest request)
        {
            var result = await _costCenterService.CreateAsync(request);
            return Ok(ApiResponse<CostCenterResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCostCenterRequest request)
        {
            var result = await _costCenterService.UpdateAsync(id, request);
            return Ok(ApiResponse<CostCenterResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _costCenterService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _costCenterService.GetByIdAsync(id);
            return Ok(ApiResponse<CostCenterResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CostCenterFilterRequest filter)
        {
            var result = await _costCenterService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<CostCenterResponse>>.SuccessResponse(result));
        }
    }
}