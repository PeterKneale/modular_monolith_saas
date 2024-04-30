using Micro.Users.Domain.Users;

namespace Micro.Users.UnitTests.Users;

[TestSubject(typeof(User))]
public class UserVerificationTests
{
    [Fact]
    public void Users_are_initially_unverified()
    {
        // arrange
        var userId = UserId.Create();
        var userName = Name.Create("first", "last");
        var email = EmailAddress.Create("user@example.com");
        var password = Password.Create("password");
        var service = new DummyPasswordService();

        // act
        var user = User.Create(userId, userName, email, password, service);

        // assert
        user.VerificationToken.Should().NotBeNull();
        user.IsVerified.Should().BeFalse();
        user.VerifiedAt.Should().BeNull();
    }

    [Fact]
    public void Users_can_be_verified()
    {
        // arrange
        var userId = UserId.Create();
        var userName = Name.Create("first", "last");
        var email = EmailAddress.Create("user@example.com");
        var password = Password.Create("password");
        var service = new DummyPasswordService();

        // act
        var user = User.Create(userId, userName, email, password, service);
        var verificationToken = user.VerificationToken!;
        user.Verify(verificationToken);

        // assert
        user.VerificationToken.Should().BeNull();
        user.IsVerified.Should().BeTrue();
        user.VerifiedAt.Should().NotBeNull();
    }
}