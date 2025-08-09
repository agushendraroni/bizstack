using AutoMapper;
using UserService.DTOs.UserActivityLog;
using UserService.Models;

namespace UserService.Mapping;

public class UserActivityLogProfile : Profile
{
    public UserActivityLogProfile()
    {
        CreateMap<CreateUserActivityLogRequest, UserActivityLog>();
        CreateMap<UserActivityLog, UserActivityLogResponse>();
    }
}