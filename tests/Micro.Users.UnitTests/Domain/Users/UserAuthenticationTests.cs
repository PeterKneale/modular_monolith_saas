namespace Micro.Users.UnitTests.Domain.Users;

[TestSubject(typeof(User))]
public class UserVerificationTests
{
    [Fact]
    public void Users_are_initially_unverified()
    {
        // arrange
        var userId = UserId.Create();
        var userName = UserName.Create("first", "last");
        var email = "user@example.com";
        var password = "password";
        var credentials = new UserCredentials(EmailAddress.Create(email), Password.Create(password));

        // act
        var user = User.CreateInstance(userId, userName, credentials);
        
        // assert
        user.Verification.VerificationToken.Should().NotBeNull();
        user.Verification.IsVerified.Should().BeFalse();
        user.Verification.VerifiedAt.Should().BeNull();
    }
    
    [Fact]
    public void Users_can_be_verified()
    {
        // arrange
        var userId = UserId.Create();
        var userName = UserName.Create("first", "last");
        var email = "user@example.com";
        var password = "password";
        var credentials = new UserCredentials(EmailAddress.Create(email), Password.Create(password));

        // act
        var user = User.CreateInstance(userId, userName, credentials);
        var verificationToken = user.Verification.VerificationToken;
        user.Verification.Verify(verificationToken);
        
        // assert
        user.Verification.VerificationToken.Should().BeNull();
        user.Verification.IsVerified.Should().BeTrue();
        user.Verification.VerifiedAt.Should().NotBeNull();
    }
}

[TestSubject(typeof(User))]
public class UserAuthenticationTests
{
    [Fact]
    public void Verified_users_can_login()
    {
        // arrange
        var userId = UserId.Create();
        var userName = UserName.Create("first","last");
        var email = "user@example.com";
        var password = "password";
        var credentials = new UserCredentials(EmailAddress.Create(email), Password.Create(password));
        
        // act
        var user = User.CreateInstance(userId, userName, credentials);
        var token = user.Verification.VerificationToken;
        user.Verification.Verify(token);
        user.CanLogin(credentials).Should().BeTrue();
    }
    
    [Fact]
    public void Unverified_users_can_not_login()
    {
        // arrange
        var userId = UserId.Create();
        var userName = UserName.Create("first","last");
        var email = "user@example.com";
        var password = "password";
        var credentials = new UserCredentials(EmailAddress.Create(email), Password.Create(password));
        
        // act
        var user = User.CreateInstance(userId, userName, credentials);
        user.CanLogin(credentials).Should().BeFalse();
    }
}