namespace AuthService.DTOs.Menu
{
    public class CreateMenuRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }
        public int CompanyId { get; set; }
    }
}