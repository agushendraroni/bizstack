using AutoMapper;
using OrganizationService.DTOs.JobTitle;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles
{
    public class JobTitleProfile : Profile
    {
        public JobTitleProfile()
        {
            CreateMap<CreateJobTitleRequest, JobTitle>();
            CreateMap<UpdateJobTitleRequest, JobTitle>();
            CreateMap<JobTitle, JobTitleResponse>();
        }
    }
}