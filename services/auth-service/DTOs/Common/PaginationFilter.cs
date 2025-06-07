using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Common;

public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public PaginationFilter()
    {
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public bool IsValid() => PageNumber > 0 && PageSize > 0;


    [StringLength(50, ErrorMessage = "SortBy maksimal 50 karakter.")]
    public string? SortBy { get; set; }

    [RegularExpression("asc|desc", ErrorMessage = "SortOrder harus 'asc' atau 'desc'.")]
    public string? SortOrder { get; set; } = "asc";
    
}