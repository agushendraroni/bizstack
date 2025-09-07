using FileStorageService.DTOs;
using SharedLibrary.DTOs;

namespace FileStorageService.Services;

public interface IFileStorageService
{
    Task<ApiResponse<FileDto>> UploadFileAsync(IFormFile file, string? category = null);
    Task<ApiResponse<List<FileDto>>> GetFilesAsync(string? category = null);
    Task<ApiResponse<FileDto>> GetFileByIdAsync(Guid id);
    Task<ApiResponse<string>> DeleteFileAsync(Guid id);
    Task<ApiResponse<Stream>> DownloadFileAsync(Guid id);
}