using System.Threading.Tasks;
using OrganizationService.DTOs.Company;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateAsync(CreateCompanyRequest request, int? tenantId = null, Guid? userId = null);
        Task<CompanyResponse> UpdateAsync(Guid id, UpdateCompanyRequest request);
        Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
        Task<CompanyResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<CompanyResponse>> GetAllAsync(CompanyFilterRequest filter);
    }
}
