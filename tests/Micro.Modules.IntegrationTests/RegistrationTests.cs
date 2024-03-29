using Micro.Modules.IntegrationTests.Fixtures;
using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;
using Micro.Users.Application.Users.Commands;

namespace Micro.Modules.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class RegistrationTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Registering_synchronises_users()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = $"test{userId.ToString()}@example.com";
        var firstName = "a";
        var lastName = "a";

        // act
        await Service.CommandUsers(new RegisterUser.Command(userId, firstName, lastName, email, "password"));
        await Service.CommandUsers(new ProcessOutboxCommand());
        await Service.CommandTenants(new ProcessInboxCommand());
        await Service.CommandTranslations(new ProcessInboxCommand());
        
        // assert
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be($"{firstName} {lastName}");
    }
    
    [Fact]
    public async Task Registering_sends_email()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = $"test{userId.ToString()}@example.com";
        var firstName = "b";
        var lastName = "b";

        // act
        await Service.CommandUsers(new RegisterUser.Command(userId, firstName, lastName, email, "password"));
        await Service.CommandUsers(new ProcessQueueCommand());

        // assert
        
    }
}