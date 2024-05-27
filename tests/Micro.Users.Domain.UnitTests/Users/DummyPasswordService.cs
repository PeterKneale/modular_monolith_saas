using Micro.Users.Domain.Users;
using Micro.Users.Domain.Users.Services;

namespace Micro.Users.UnitTests.Users;

internal class DummyPasswordService : IHashPassword, ICheckPassword
{
    public bool Matches(Password password, PasswordHash passwordHash) => password.Value == passwordHash.Value;
    public PasswordHash HashPassword(Password password) => new(password);
}