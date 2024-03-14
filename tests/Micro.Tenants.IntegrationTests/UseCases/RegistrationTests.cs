using Micro.Tenants.Application.Users.Commands;
using Micro.Tenants.Application.Users.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class RegistrationTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Registering_and_verification_allows_login()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = $"test{Guid.NewGuid().ToString()}@example.org";
        var password = "password";
        var register = new RegisterUser.Command(userId, "x", "x", email, password);

        // act
        await Service.Command(register);
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        await Service.Command(new VerifyUser.Command(userId, token));
        var results = await Service.Query(new CanAuthenticate.Query(email, password));

        // assert
        results.Success.Should().BeTrue();
        results.UserId.Should().Be(userId);
    }
}