using AutoMapper;
using OrganizationService.DTOs.Branch;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<CreateBranchRequest, Branch>();
            CreateMap<UpdateBranchRequest, Branch>();
            CreateMap<Branch, BranchResponse>();
        }
    }
}