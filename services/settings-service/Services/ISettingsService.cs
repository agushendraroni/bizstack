using SettingsService.Models;
using SharedLibrary.DTOs;

namespace SettingsService.Services;

public interface ISettingsService
{
    Task<ApiResponse<List<MenuItem>>> GetMenuItemsAsync(string? context = null);
    Task<ApiResponse<List<MenuItem>>> GetMenuTreeAsync(string context);
    Task<ApiResponse<MenuItem>> CreateMenuItemAsync(MenuItem menuItem);
    Task<ApiResponse<MenuItem>> UpdateMenuItemAsync(Guid id, MenuItem menuItem);
    Task<ApiResponse<string>> DeleteMenuItemAsync(Guid id);
    
    Task<ApiResponse<List<SystemSetting>>> GetSystemSettingsAsync();
    Task<ApiResponse<SystemSetting>> GetSystemSettingAsync(string key);
    Task<ApiResponse<SystemSetting>> UpdateSystemSettingAsync(string key, string value);
}