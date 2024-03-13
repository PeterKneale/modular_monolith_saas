using Micro.Tenants.IntegrationEvents;
using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Translations.IntegrationTests.UseCases.Integration;

[Collection(nameof(ServiceFixtureCollection))]
public class UserCreatedHandlerTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_be_handled()
    {
        // arrange 
        var userId = Guid.NewGuid();
        var integrationEvent = new UserCreated { UserId = userId, Name = "X" };

        // act
        await Service.Publish(integrationEvent);

        // assert
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user.Name.Should().Be("X");
    }
}