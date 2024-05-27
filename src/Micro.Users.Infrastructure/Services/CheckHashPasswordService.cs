using Micro.Users.Domain.Users;
using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Infrastructure.Services;

internal class CheckHashPasswordService : IHashPassword, ICheckPassword
{
    public bool Matches(Password password, PasswordHash passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password.Value, passwordHash.Value);

    public PasswordHash HashPassword(Password password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password.Value);
        return new PasswordHash(hash);
    }
}