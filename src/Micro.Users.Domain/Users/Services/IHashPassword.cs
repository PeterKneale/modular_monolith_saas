namespace Micro.Users.Domain.Users.Services;

public interface IHashPassword
{
    PasswordHash HashPassword(Password password);
}