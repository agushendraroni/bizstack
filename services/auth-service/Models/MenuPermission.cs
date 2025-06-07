using AuthService.Models.Base;

namespace AuthService.Models
{
    public class MenuPermission: BaseEntity
    {
        
        public int MenuId { get; set; }
        public Menu? Menu { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
