namespace Micro.Users.Domain.Users.Services;

public interface ICheckPassword
{
    bool Matches(Password password, PasswordHash passwordHash);
}