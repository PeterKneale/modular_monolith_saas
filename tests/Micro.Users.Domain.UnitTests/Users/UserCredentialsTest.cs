using Micro.Users.Domain.Users;

namespace Micro.Users.UnitTests.Users;

public class UserLoginTest
{
    [Theory]
    [InlineData("user@example.com", "X", "user@example.com", "X", true, "matches")]
    [InlineData("user@example.com", "X", "user@example.com", "X", true, "emails are case insensitive")]
    [InlineData("user@example.com", "X", "user@example.com", "WRONG_PASSWORD", false, "right email but wrong password")]
    [InlineData("user@example.com", "X", "WRONG_EMAIL@example.com", "X", false, "wrong email but right password")]
    [InlineData("user@example.com", "X", "WRONG_EMAIL@example.com", "WRONG_PASSWORD", false, "wrong email but right password")]
    public void CredentialScenarios(string email1, string password1, string email2, string password2, bool expected, string because)
    {
        // arrange
        var emailAddress = EmailAddress.Create(email1);
        var password = Password.Create(password1);
        var userId = UserId.Create();
        var userName = Name.Create("x", "x");
        var service = new DummyPasswordService();

        // act
        var user = User.Create(userId, userName, emailAddress, password, service);
        user.Verify(user.VerificationToken!);
        var action = () => user.Login(EmailAddress.Create(email2), Password.Create(password2), service);
        
        // assert
        if (expected)
            action.Should().NotThrow(because);
        else
            action.Should().Throw<BusinessRuleBrokenException>(because);
    }
}