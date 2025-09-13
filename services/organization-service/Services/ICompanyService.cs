using OrganizationService.DTOs;
using SharedLibrary.DTOs;

namespace OrganizationService.Services;

public interface ICompanyService
{
    Task<ApiResponse<IEnumerable<CompanyDto>>> GetAllCompaniesAsync(int? tenantId = null);
    Task<ApiResponse<CompanyDto>> GetCompanyByIdAsync(int tenantId);
    Task<ApiResponse<CompanyDto>> GetCompanyByCodeAsync(string code);
    Task<ApiResponse<CompanyDto>> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
    Task<ApiResponse<CompanyDto>> UpdateCompanyAsync(int tenantId, UpdateCompanyDto updateCompanyDto);
    Task<ApiResponse<bool>> DeleteCompanyAsync(int tenantId);
}