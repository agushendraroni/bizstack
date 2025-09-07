using AutoMapper;
using OrganizationService.DTOs;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles;

public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
        // Company mappings
        CreateMap<Company, CompanyDto>();
        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Additional mappings can be added here as needed
    }
}
