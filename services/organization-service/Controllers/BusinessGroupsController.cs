using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.DTOs.BusinessGroup;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusinessGroupsController : ControllerBase
    {
        private readonly IBusinessGroupService _businessGroupService;

        public BusinessGroupsController(IBusinessGroupService businessGroupService)
        {
            _businessGroupService = businessGroupService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessGroupRequest request)
        {
            var result = await _businessGroupService.CreateAsync(request);
            return Ok(ApiResponse<BusinessGroupResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBusinessGroupRequest request)
        {
            var result = await _businessGroupService.UpdateAsync(id, request);
            return Ok(ApiResponse<BusinessGroupResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _businessGroupService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _businessGroupService.GetByIdAsync(id);
            return Ok(ApiResponse<BusinessGroupResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BusinessGroupFilterRequest filter)
        {
            var result = await _businessGroupService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<BusinessGroupResponse>>.SuccessResponse(result));
        }
    }
}