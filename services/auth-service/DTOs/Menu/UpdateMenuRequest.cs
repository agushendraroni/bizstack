
// DTOs/Menu/UpdateMenuRequest.cs
namespace AuthService.DTOs.Menu
{
    public class UpdateMenuRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }
    }
}