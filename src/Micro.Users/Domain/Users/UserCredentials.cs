namespace Micro.Users.Domain.Users;

public class UserCredentials(EmailAddress email, Password password)
{
    public EmailAddress Email { get; } = email;
    public Password Password { get; private set; } = password;

    internal bool Matches(UserCredentials credentials) =>
        Email.Matches(credentials.Email) &&
        Password.Matches(credentials.Password);

    public void ChangePassword(Password password)
    {
        Password = password;
    }

    public override string ToString() => $"{Email}";
}