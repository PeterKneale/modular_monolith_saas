using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;
using Micro.Users.Messages;

namespace Micro.Translations.IntegrationTests.UseCases.Integration;

[Collection(nameof(ServiceFixtureCollection))]
public class UserChangedHandlerTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_be_handled()
    {
        // arrange 
        var userId = Guid.NewGuid();
        var integrationEvent1 = new UserCreated { UserId = userId, Name = "X" };
        var integrationEvent2 = new UserChanged { UserId = userId, Name = "Y" };

        // act
        await Service.Publish(integrationEvent1);
        await Service.Publish(integrationEvent2);

        // assert
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be("Y");
    }
}