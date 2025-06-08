using AuthService.Models.Base;

namespace AuthService.DTOs.Menu
{
    public class MenuResponse:BaseEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        public int OrderIndex { get; set; } = 0;
    }
}
