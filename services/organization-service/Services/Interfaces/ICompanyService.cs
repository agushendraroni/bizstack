using System.Threading.Tasks;
using OrganizationService.DTOs.Company;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
        Task<CompanyResponse> UpdateAsync(Guid id, UpdateCompanyRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<CompanyResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<CompanyResponse>> GetAllAsync(CompanyFilterRequest filter);
    }
}
