namespace AuthService.DTOs.Menu
{
    public class MenuResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public int CompanyId { get; set; }
    }
}