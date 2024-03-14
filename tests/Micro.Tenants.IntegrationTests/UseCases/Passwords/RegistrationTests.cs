using Micro.Tenants.Application.Users.Commands;
using Micro.Tenants.Application.Users.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Passwords;

[Collection(nameof(ServiceFixtureCollection))]
public class ChangePasswordTests(ServiceFixture service, ITestOutputHelper outputHelper)  :BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_login_after_changing_password()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = "user@example.org";
        var password1 = "password1";
        var password2 = "password2";

        // act
        await Service.Command(new RegisterUser.Command(userId, "x", "x", email, password1));
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        await Service.Command(new VerifyUser.Command(userId, token));
        await Service.Command(new UpdateUserPassword.Command(password1, password2), userId);

        // assert
        var results1 = await Service.Query(new CanAuthenticate.Query(email, password1));
        results1.Success.Should().BeFalse();
        
        var results2 = await Service.Query(new CanAuthenticate.Query(email, password2));
        results2.Success.Should().BeTrue();
        results2.UserId.Should().Be(userId);
    }
}