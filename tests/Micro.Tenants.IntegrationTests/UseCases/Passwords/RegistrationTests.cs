using Micro.Tenants.Application.Users.Commands;
using Micro.Tenants.Application.Users.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Passwords;

[Collection(nameof(ServiceFixtureCollection))]
public class ChangePasswordTests
{
    private readonly ServiceFixture _service;

    public ChangePasswordTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_login_after_changing_password()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = "user@example.org";
        var password1 = "password1";
        var password2 = "password2";

        // act
        await _service.Command(new RegisterUser.Command(userId, "x", "x", email, password1));
        var token = await _service.Query(new GetUserVerificationToken.Query(userId));
        await _service.Command(new VerifyUser.Command(userId, token));
        await _service.Command(new UpdateUserPassword.Command(password1, password2), userId);

        // assert
        var results1 = await _service.Query(new CanAuthenticate.Query(email, password1));
        results1.Success.Should().BeFalse();
        
        var results2 = await _service.Query(new CanAuthenticate.Query(email, password2));
        results2.Success.Should().BeTrue();
        results2.UserId.Should().Be(userId);
    }
}