namespace UserService.DTOs
{
    public class UpdateUserRequest
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }

         public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
