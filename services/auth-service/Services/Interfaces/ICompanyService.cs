using AuthService.DTOs.Company;
using SharedLibrary.DTOs;

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
