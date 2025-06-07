namespace AuthService.DTOs.Menu
{
    public class MenuFilterRequest
    {
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
    }
}