namespace UserService.DTOs
{
    public class CreateUserRequest
    {
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
