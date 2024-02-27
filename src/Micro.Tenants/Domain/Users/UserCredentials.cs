namespace Micro.Tenants.Domain.Users;

public class UserCredentials(EmailAddress email, Password password)
{
    public EmailAddress Email { get; init; } = email;
    public Password Password { get; init; } = password;

    public bool Match(UserCredentials credentials)
    {
        return credentials.Email == Email && credentials.Password == Password; 
    }
}