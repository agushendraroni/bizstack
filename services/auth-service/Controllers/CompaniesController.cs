using AuthService.DTOs.Company;
using AuthService.DTOs.Common;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request)
        {
            var result = await _companyService.CreateAsync(request);
            return Ok(ApiResponse<CompanyResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyRequest request)
        {
            var result = await _companyService.UpdateAsync(id, request);
            return Ok(ApiResponse<CompanyResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _companyService.GetByIdAsync(id);
            return Ok(ApiResponse<CompanyResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CompanyFilterRequest filter)
        {
            var result = await _companyService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<CompanyResponse>>.SuccessResponse(result));
        }
    }
}