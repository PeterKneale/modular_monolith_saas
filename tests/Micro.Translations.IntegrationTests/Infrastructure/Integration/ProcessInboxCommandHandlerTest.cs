using JetBrains.Annotations;
using Micro.Tenants.IntegrationEvents;
using Micro.Translations.Infrastructure.Integration;

namespace Micro.Translations.IntegrationTests.Infrastructure.Integration;

[TestSubject(typeof(ProcessInboxCommandHandler))]
[Collection(nameof(ServiceFixtureCollection))]
public class ProcessInboxCommandHandlerTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Inbox_can_be_processed()
    {
        // arrange
        await IntegrationHelper.PurgeInbox();
        await IntegrationHelper.PushMessageIntoInbox(new UserCreated { UserId = Guid.NewGuid(), Name = "X" });
        (await IntegrationHelper.CountPendingInboxMessages()).Should().Be(1);

        // act
        await Service.Command(new ProcessInboxCommand());
        
        // assert
        (await IntegrationHelper.CountPendingInboxMessages()).Should().Be(0);
    }
}