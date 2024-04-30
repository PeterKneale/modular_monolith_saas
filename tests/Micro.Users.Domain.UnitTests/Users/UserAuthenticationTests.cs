using Micro.Users.Domain.Users;

namespace Micro.Users.UnitTests.Users;

[TestSubject(typeof(User))]
public class UserAuthenticationTests
{
    [Fact]
    public void Verified_users_can_login()
    {
        // arrange
        var userId = UserId.Create();
        var userName = Name.Create("first", "last");
        var email = EmailAddress.Create("user@example.com");
        var password = Password.Create("password");
        var service = new DummyPasswordService();

        // act
        var user = User.Create(userId, userName, email, password, service);
        var token = user.VerificationToken!;
        user.Verify(token);
        user.Login(email, password, service);
    }

    [Fact]
    public void Unverified_users_can_not_login()
    {
        // arrange
        var userId = UserId.Create();
        var userName = Name.Create("first", "last");
        var email = EmailAddress.Create("user@example.com");
        var password = Password.Create("password");
        var service = new DummyPasswordService();

        // act
        var user = User.Create(userId, userName, email, password, service);
        var action = () => user.Login(email, password, service);

        // assert
        action.Should().Throw<BusinessRuleBrokenException>();
    }
}