using OrganizationService.DTOs.JobTitle;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface IJobTitleService
    {
        Task<JobTitleResponse> CreateAsync(CreateJobTitleRequest request, int? tenantId = null, Guid? userId = null);
        Task<JobTitleResponse> UpdateAsync(Guid id, UpdateJobTitleRequest request);
        Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
        Task<JobTitleResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<JobTitleResponse>> GetAllAsync(JobTitleFilterRequest filter);
    }
}