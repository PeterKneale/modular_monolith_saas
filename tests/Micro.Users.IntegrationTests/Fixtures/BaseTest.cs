using Xunit.Abstractions;

namespace Micro.Users.IntegrationTests.Fixtures;

public class BaseTest
{
    protected ServiceFixture Service { get; }

    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        Service = service;
    }

    protected async Task<Guid> RegisterAndVerifyUser(string? email = null, string? password = null)
    {
        var userId = await RegisterUser(email, password);
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        await Service.Command(new VerifyUser.Command(userId, token));
        return userId;
    }

    protected async Task<Guid> RegisterUser(string? email = null, string? password = null)
    {
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId, email, password);
        await Service.Command(register);
        return userId;
    }
}