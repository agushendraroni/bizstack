using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Common;

public class PaginationFilter
{
    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = (value <= 0) ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value <= 0) ? 10 : value;
    }

    [StringLength(50, ErrorMessage = "SortBy maksimal 50 karakter.")]
    public string? SortBy { get; set; }

    [RegularExpression("asc|desc", ErrorMessage = "SortOrder harus 'asc' atau 'desc'.")]
    public string? SortOrder { get; set; } = "asc";

}

public class PaginatedResponse<T>
{
    public List<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }


    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public PaginatedResponse(List<T> data, int totalCount, int page, int pageSize)
    {
        Data = data;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

}