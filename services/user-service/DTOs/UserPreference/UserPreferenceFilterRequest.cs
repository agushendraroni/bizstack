
namespace UserService.DTOs.UserPreference;
using System;
using SharedLibrary.DTOs;


public class UserPreferenceFilterRequest : PaginationFilter
{
    public string? Language { get; set; }
    public string? Theme { get; set; }
    public bool? ReceiveNotifications { get; set; }
    public bool? IsActive { get; set; }
}
