using Micro.IntegrationTests.Fixtures;
using Micro.Tenants.Application.Users.Commands;
using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;

namespace Micro.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class RegistrationTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Registering_synchronises_users()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = $"test{userId.ToString()}@example.com";
        var firstName = "first";
        var lastName = "last";

        // act
        await Service.CommandTenants(new RegisterUser.Command(userId, firstName, lastName, email, "password"));
        await Service.CommandTenants(new ProcessOutboxCommand());
        await Service.CommandTranslations(new ProcessInboxCommand());

        // assert
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be($"{firstName} {lastName}");
    }
}