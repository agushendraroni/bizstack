namespace SharedLibrary.Security.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Role { get; }
    string? CompanyId { get; }
}
