using AutoMapper;
using OrganizationService.DTOs.Division;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles
{
    public class DivisionProfile : Profile
    {
        public DivisionProfile()
        {
            CreateMap<CreateDivisionRequest, Division>();
            CreateMap<UpdateDivisionRequest, Division>();
            CreateMap<Division, DivisionResponse>();
        }
    }
}