using SharedLibrary.DTOs;

namespace AuthService.DTOs.Menu
{
    public class MenuFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
    }
}





    