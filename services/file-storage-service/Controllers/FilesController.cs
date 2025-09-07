using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using FileStorageService.Data;
using FileStorageService.DTOs;
using FileStorageService.Models;
using SharedLibrary.DTOs;

namespace FileStorageService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FilesController : ControllerBase
{
    private readonly FileStorageDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public FilesController(FileStorageDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles([FromQuery] string? category)
    {
        var query = _context.FileRecords.Where(f => f.IsActive);
        
        if (!string.IsNullOrEmpty(category))
            query = query.Where(f => f.Category == category);

        var files = await query
            .OrderByDescending(f => f.UploadedAt)
            .Select(f => new FileRecordDto
            {
                Id = f.Id,
                FileName = f.FileName,
                OriginalFileName = f.OriginalFileName,
                FilePath = f.FilePath,
                ContentType = f.ContentType,
                FileSize = f.FileSize,
                Category = f.Category,
                Description = f.Description,
                RelatedEntityId = f.RelatedEntityId,
                RelatedEntityType = f.RelatedEntityType,
                UploadedAt = f.UploadedAt,
                UploadedBy = f.UploadedBy,
                FileUrl = $"/uploads/{f.FileName}"
            })
            .ToListAsync();

        return Ok(ApiResponse<List<FileRecordDto>>.Success(files));
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest(ApiResponse<FileUploadResultDto>.Error("No file provided"));

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "application/pdf", "text/plain" };
        if (!allowedTypes.Contains(dto.File.ContentType))
            return BadRequest(ApiResponse<FileUploadResultDto>.Error("File type not allowed"));

        var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        var fileRecord = new FileRecord
        {
            FileName = fileName,
            OriginalFileName = dto.File.FileName,
            FilePath = filePath,
            ContentType = dto.File.ContentType,
            FileSize = dto.File.Length,
            Category = dto.Category,
            Description = dto.Description,
            RelatedEntityId = dto.RelatedEntityId,
            RelatedEntityType = dto.RelatedEntityType,
            UploadedBy = dto.UploadedBy
        };

        _context.FileRecords.Add(fileRecord);
        await _context.SaveChangesAsync();

        var result = new FileUploadResultDto
        {
            FileId = fileRecord.Id,
            FileName = fileName,
            FileUrl = $"/uploads/{fileName}",
            FileSize = dto.File.Length,
            ContentType = dto.File.ContentType
        };

        return Ok(ApiResponse<FileUploadResultDto>.Success(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(Guid id)
    {
        var fileRecord = await _context.FileRecords.FindAsync(id);
        if (fileRecord == null || !fileRecord.IsActive)
            return NotFound(ApiResponse<FileRecordDto>.Error("File not found"));

        var fileDto = new FileRecordDto
        {
            Id = fileRecord.Id,
            FileName = fileRecord.FileName,
            OriginalFileName = fileRecord.OriginalFileName,
            FilePath = fileRecord.FilePath,
            ContentType = fileRecord.ContentType,
            FileSize = fileRecord.FileSize,
            Category = fileRecord.Category,
            Description = fileRecord.Description,
            RelatedEntityId = fileRecord.RelatedEntityId,
            RelatedEntityType = fileRecord.RelatedEntityType,
            UploadedAt = fileRecord.UploadedAt,
            UploadedBy = fileRecord.UploadedBy,
            FileUrl = $"/uploads/{fileRecord.FileName}"
        };

        return Ok(ApiResponse<FileRecordDto>.Success(fileDto));
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(Guid id)
    {
        var fileRecord = await _context.FileRecords.FindAsync(id);
        if (fileRecord == null || !fileRecord.IsActive)
            return NotFound();

        if (!System.IO.File.Exists(fileRecord.FilePath))
            return NotFound();

        var fileBytes = await System.IO.File.ReadAllBytesAsync(fileRecord.FilePath);
        return File(fileBytes, fileRecord.ContentType, fileRecord.OriginalFileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(Guid id)
    {
        var fileRecord = await _context.FileRecords.FindAsync(id);
        if (fileRecord == null)
            return NotFound(ApiResponse<string>.Error("File not found"));

        fileRecord.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success("File deleted successfully"));
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.FileRecords
            .Where(f => f.IsActive)
            .GroupBy(f => f.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToListAsync();

        return Ok(ApiResponse<object>.Success(categories));
    }

    private int? GetTenantId()
    {
        if (Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader) && 
            int.TryParse(tenantIdHeader.FirstOrDefault(), out var tenantId))
        {
            return tenantId;
        }
        return null;
    }

    private Guid? GetUserId()
    {
        if (Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) && 
            Guid.TryParse(userIdHeader.FirstOrDefault(), out var userId))
        {
            return userId;
        }
        return null;
    }
}
