using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Infrastructure;
using Micro.Translations.Infrastructure;
using Micro.Users.Application.Users.Commands;
using Micro.Users.Infrastructure;

namespace Micro.Modules.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class IntegrationTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Users_organisations_and_projects_are_synchronised_to_other_modules()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = $"test{userId.ToString()}@example.com";
        var firstName = "a";
        var lastName = "a";
        var organisationId = Guid.NewGuid();
        var organisation = $"Organisation-{organisationId}";
        var projectId = Guid.NewGuid();
        var project = $"Project-{projectId}";

        // act
        await Service.CommandUsers(new RegisterUser.Command(userId, firstName, lastName, email, "password"));
        await Service.CommandTenants(new CreateOrganisation.Command(organisationId, organisation), userId);
        await Service.CommandTenants(new CreateProject.Command(projectId, project), userId, organisationId);
        await Service.CommandUsers(new ProcessOutboxCommand()); // user out
        await Service.CommandTenants(new ProcessInboxCommand()); // user in
        await Service.CommandTenants(new ProcessOutboxCommand()); // tenant out
        await Service.CommandTranslations(new ProcessInboxCommand()); // user in, tenant in, project in

        // assert
        await AssertUserPresenceInUsersModule(userId, firstName, lastName);
        await AssertUserSyncToTenantsModule(userId, firstName, lastName);
        await AssertUserSyncToTranslationsModule(userId, firstName, lastName);
        await AssertProjectSyncToTranslationsModule(projectId, project);
    }
    
    private static async Task AssertUserPresenceInUsersModule(Guid userId, string firstName, string lastName)
    {
        using var scope = UsersCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Users.Infrastructure.Database.Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.First.Should().Be(firstName);
        user!.Name.Last.Should().Be(lastName);
    }

    private static async Task AssertProjectSyncToTranslationsModule(Guid projectId, string name)
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Translations.Infrastructure.Database.Db>();
        var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
        project.Should().NotBeNull();
        project!.Name.Should().Be(name);
    }

    private static async Task AssertUserSyncToTenantsModule(Guid userId, string firstName, string lastName)
    {
        using var scope = TenantsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Tenants.Infrastructure.Database.Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be($"{firstName} {lastName}");
    }

    private static async Task AssertUserSyncToTranslationsModule(Guid userId, string firstName, string lastName)
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Translations.Infrastructure.Database.Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be($"{firstName} {lastName}");
    }
}