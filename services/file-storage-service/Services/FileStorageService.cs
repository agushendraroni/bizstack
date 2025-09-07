using Microsoft.EntityFrameworkCore;
using FileStorageService.Data;
using FileStorageService.DTOs;
using FileStorageService.Models;
using SharedLibrary.DTOs;

namespace FileStorageService.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public FileStorageService(FileStorageDbContext context, IWebHostEnvironment environment, IConfiguration configuration)
    {
        _context = context;
        _environment = environment;
        _configuration = configuration;
    }

    public async Task<ApiResponse<FileDto>> UploadFileAsync(IFormFile file, string? category = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return new ApiResponse<FileDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "No file provided"
                };
            }

            var maxFileSize = _configuration.GetValue<long>("FileStorage:MaxFileSize", 10485760); // 10MB default
            if (file.Length > maxFileSize)
            {
                return new ApiResponse<FileDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = $"File size exceeds maximum allowed size of {maxFileSize / 1024 / 1024}MB"
                };
            }

            var allowedExtensions = _configuration.GetSection("FileStorage:AllowedExtensions").Get<string[]>() 
                ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx" };
            
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return new ApiResponse<FileDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = $"File type {fileExtension} is not allowed"
                };
            }

            var uploadPath = _configuration.GetValue<string>("FileStorage:UploadPath", "wwwroot/uploads");
            var fullUploadPath = Path.Combine(_environment.ContentRootPath, uploadPath);
            
            if (!Directory.Exists(fullUploadPath))
            {
                Directory.CreateDirectory(fullUploadPath);
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(fullUploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileRecord = new FileRecord
            {
                FileName = fileName,
                OriginalFileName = file.FileName,
                FilePath = filePath,
                FileSize = file.Length,
                ContentType = file.ContentType,
                Category = category ?? "general",
                UploadedAt = DateTime.UtcNow
            };

            _context.FileRecords.Add(fileRecord);
            await _context.SaveChangesAsync();

            var result = new FileDto
            {
                Id = fileRecord.Id,
                FileName = fileRecord.FileName,
                OriginalFileName = fileRecord.OriginalFileName,
                FileSize = fileRecord.FileSize,
                ContentType = fileRecord.ContentType,
                Category = fileRecord.Category,
                UploadedAt = fileRecord.UploadedAt,
                FileUrl = $"/api/files/{fileRecord.Id}/download"
            };

            return new ApiResponse<FileDto>
            {
                Data = result,
                IsSuccess = true,
                Message = "File uploaded successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FileDto>
            {
                Data = null,
                IsSuccess = false,
                Message = $"Failed to upload file: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<List<FileDto>>> GetFilesAsync(string? category = null)
    {
        try
        {
            var query = _context.FileRecords.AsQueryable();
            
            if (!string.IsNullOrEmpty(category))
                query = query.Where(f => f.Category == category);

            var files = await query
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();

            var result = files.Select(f => new FileDto
            {
                Id = f.Id,
                FileName = f.FileName,
                OriginalFileName = f.OriginalFileName,
                FileSize = f.FileSize,
                ContentType = f.ContentType,
                Category = f.Category,
                UploadedAt = f.UploadedAt,
                FileUrl = $"/api/files/{f.Id}/download"
            }).ToList();

            return new ApiResponse<List<FileDto>>
            {
                Data = result,
                IsSuccess = true,
                Message = "Files retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<FileDto>>
            {
                Data = new List<FileDto>(),
                IsSuccess = false,
                Message = $"Failed to get files: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<FileDto>> GetFileByIdAsync(Guid id)
    {
        try
        {
            var file = await _context.FileRecords.FindAsync(id);
            
            if (file == null)
            {
                return new ApiResponse<FileDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "File not found"
                };
            }

            var result = new FileDto
            {
                Id = file.Id,
                FileName = file.FileName,
                OriginalFileName = file.OriginalFileName,
                FileSize = file.FileSize,
                ContentType = file.ContentType,
                Category = file.Category,
                UploadedAt = file.UploadedAt,
                FileUrl = $"/api/files/{file.Id}/download"
            };

            return new ApiResponse<FileDto>
            {
                Data = result,
                IsSuccess = true,
                Message = "File retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FileDto>
            {
                Data = null,
                IsSuccess = false,
                Message = $"Failed to get file: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> DeleteFileAsync(Guid id)
    {
        try
        {
            var file = await _context.FileRecords.FindAsync(id);
            
            if (file == null)
            {
                return new ApiResponse<string>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "File not found"
                };
            }

            // Delete physical file
            if (File.Exists(file.FilePath))
            {
                File.Delete(file.FilePath);
            }

            // Delete database record
            _context.FileRecords.Remove(file);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Data = "File deleted",
                IsSuccess = true,
                Message = "File deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                Data = null,
                IsSuccess = false,
                Message = $"Failed to delete file: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<Stream>> DownloadFileAsync(Guid id)
    {
        try
        {
            var file = await _context.FileRecords.FindAsync(id);
            
            if (file == null || !File.Exists(file.FilePath))
            {
                return new ApiResponse<Stream>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "File not found"
                };
            }

            var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);
            
            return new ApiResponse<Stream>
            {
                Data = stream,
                IsSuccess = true,
                Message = "File ready for download"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Stream>
            {
                Data = null,
                IsSuccess = false,
                Message = $"Failed to download file: {ex.Message}"
            };
        }
    }
}