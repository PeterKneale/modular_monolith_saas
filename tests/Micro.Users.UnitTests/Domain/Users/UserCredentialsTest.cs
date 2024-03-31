namespace Micro.Users.UnitTests.Domain.Users;

[TestSubject(typeof(UserCredentials))]
public class UserCredentialsTest
{
    [Theory]
    [InlineData("user@example.com", "X", "user@example.com", "X", true, "matches")]
    [InlineData("user@example.com", "X", "user@example.com", "X", true, "emails are case insensitive")]
    [InlineData("user@example.com", "X", "user@example.com", "WRONG_PASSWORD", false, "right email but wrong password")]
    [InlineData("user@example.com", "X", "WRONG_EMAIL@example.com", "X", false, "wrong email but right password")]
    [InlineData("user@example.com", "X", "WRONG_EMAIL@example.com", "WRONG_PASSWORD", false, "wrong email but right password")]
    public void UserCredentialsMatchTests(string email1, string password1, string email2, string password2, bool expected, string because)
    {
        var userCredentials1 = new UserCredentials(EmailAddress.Create(email1), Password.Create(password1));
        var userCredentials2 = new UserCredentials(EmailAddress.Create(email2), Password.Create(password2));
        userCredentials1.Matches(userCredentials2).Should().Be(expected, because);
    }
}