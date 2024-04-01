namespace Micro.Users.Domain.Users.Services;

public interface IHashPassword
{
    HashedPassword HashPassword(Password password);
}