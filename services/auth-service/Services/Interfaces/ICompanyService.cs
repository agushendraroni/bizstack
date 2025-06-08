using AuthService.DTOs.Company;
using AuthService.DTOs.Common;

namespace AuthService.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
        Task<CompanyResponse> UpdateAsync(int id, UpdateCompanyRequest request);
        Task<bool> DeleteAsync(int id);
        Task<CompanyResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<CompanyResponse>> GetAllAsync(CompanyFilterRequest filter);
    }
}
