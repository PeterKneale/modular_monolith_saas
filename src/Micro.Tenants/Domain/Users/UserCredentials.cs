namespace Micro.Tenants.Domain.Users;

public class UserCredentials(string email, string password)
{
    public string Email { get; init; } = email.ToLowerInvariant();
    public string Password { get; init; } = password;

    public bool Match(UserCredentials credentials)
    {
        return credentials.Email == Email && credentials.Password == Password; 
    }
}