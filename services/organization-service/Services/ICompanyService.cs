using OrganizationService.DTOs;
using SharedLibrary.DTOs;

namespace OrganizationService.Services;

public interface ICompanyService
{
    Task<ApiResponse<IEnumerable<CompanyDto>>> GetAllCompaniesAsync();
    Task<ApiResponse<CompanyDto>> GetCompanyByIdAsync(Guid id);
    Task<ApiResponse<CompanyDto>> GetCompanyByCodeAsync(string code);
    Task<ApiResponse<CompanyDto>> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
    Task<ApiResponse<CompanyDto>> UpdateCompanyAsync(Guid id, UpdateCompanyDto updateCompanyDto);
    Task<ApiResponse<bool>> DeleteCompanyAsync(Guid id);
}
