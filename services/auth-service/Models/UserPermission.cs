using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class UserPermission : BaseEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}