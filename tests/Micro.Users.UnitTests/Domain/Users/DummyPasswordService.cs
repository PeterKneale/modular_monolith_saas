using Micro.Users.Domain.Users.Services;

namespace Micro.Users.UnitTests.Domain.Users;

internal class DummyPasswordService : IHashPassword, ICheckPassword
{
    public HashedPassword HashPassword(Password password) => new(password);

    public bool Matches(Password password, HashedPassword hashedPassword) => password.Value == hashedPassword.Value;
}