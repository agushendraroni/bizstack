using AutoMapper;
using CustomerService.DTOs;
using CustomerService.Models;

namespace CustomerService.MappingProfiles;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.CustomerGroupName, opt => opt.MapFrom(src => src.CustomerGroup != null ? src.CustomerGroup.Name : null));
        
        CreateMap<CreateCustomerDto, Customer>();
        
        CreateMap<UpdateCustomerDto, Customer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // CustomerGroup mappings
        CreateMap<CustomerGroup, CustomerGroupDto>()
            .ForMember(dest => dest.CustomerCount, opt => opt.MapFrom(src => src.Customers.Count(c => c.IsActive)));
        
        CreateMap<CreateCustomerGroupDto, CustomerGroup>();
        
        CreateMap<UpdateCustomerGroupDto, CustomerGroup>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
