using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Infrastructure.Services;

internal class CheckHashPasswordService : IHashPassword, ICheckPassword
{
    public HashedPassword HashPassword(Password password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password.Value);
        return new HashedPassword(hash);
    }
    
    public bool Matches(Password password, HashedPassword hashedPassword) => 
        BCrypt.Net.BCrypt.Verify(password.Value, hashedPassword.Value);
}