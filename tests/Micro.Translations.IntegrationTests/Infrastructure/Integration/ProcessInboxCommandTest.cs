using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;
using Micro.Users.IntegrationEvents;

namespace Micro.Translations.IntegrationTests.Infrastructure.Integration;

[TestSubject(typeof(ProcessInboxCommand))]
[Collection(nameof(ServiceFixtureCollection))]
public class ProcessInboxCommandTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Inbox_can_be_processed()
    {
        // arrange
        var userId = Guid.NewGuid();
        var name = "X";
        await IntegrationHelper.PushMessageIntoInbox(new UserCreated { UserId = userId, Name = name });
        
        // act
        await Service.Command(new ProcessInboxCommand());

        // assert
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be(name);
    }
}