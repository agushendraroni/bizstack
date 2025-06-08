
// DTOs/Menu/UpdateMenuRequest.cs
namespace AuthService.DTOs.Menu
{
    public class UpdateMenuRequest
    {
         public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        public int OrderIndex { get; set; } = 0;
    }
}