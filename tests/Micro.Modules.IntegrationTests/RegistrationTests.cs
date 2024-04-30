using Micro.Tenants.Infrastructure;
using Micro.Translations.Infrastructure;
using Micro.Users.Application.Users.Commands;
using Micro.Users.Infrastructure;

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
        await AssertPresenceInUsersModule(userId, firstName, lastName);
        await AssertSyncToTenantsModule(userId, firstName, lastName);
        await AssertSyncToTranslationsModule(userId, firstName, lastName);
    }
    
    private static async Task AssertPresenceInUsersModule(Guid userId, string firstName, string lastName)
    {
        using var scope = UsersCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Users.Infrastructure.Database.Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.First.Should().Be(firstName);
        user!.Name.Last.Should().Be(lastName);
    }

    private static async Task AssertSyncToTenantsModule(Guid userId, string firstName, string lastName)
    {
        using var scope = TenantsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Tenants.Infrastructure.Database.Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be($"{firstName} {lastName}");
    }
    
    private static async Task AssertSyncToTranslationsModule(Guid userId, string firstName, string lastName)
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Translations.Infrastructure.Database.Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be($"{firstName} {lastName}");
    }
}