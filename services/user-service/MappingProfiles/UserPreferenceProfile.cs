using AutoMapper;
using UserService.DTOs.UserPreference;
using UserService.Models;

namespace UserService.Mapping;

public class UserPreferenceProfile : Profile
{
    public UserPreferenceProfile()
    {
        CreateMap<CreateUserPreferenceRequest, UserPreference>();
        CreateMap<UpdateUserPreferenceRequest, UserPreference>();
        CreateMap<UserPreference, UserPreferenceResponse>();
    }
}
