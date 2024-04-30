using Micro.Users.Messages;

namespace Micro.Users.IntegrationTests.Infrastructure.Integration;

[Collection(nameof(ServiceFixtureCollection))]
public class ProcessOutboxCommandHandlerTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Outbox_can_be_processed()
    {
        // arrange
        await IntegrationHelper.PurgeOutbox();
        await IntegrationHelper.PushMessageIntoOutbox(new UserCreated { UserId = Guid.NewGuid(), Name = "X" });
        (await IntegrationHelper.CountPendingOutboxMessages()).Should().Be(1);

        // act
        await Service.Command(new ProcessOutboxCommand());

        // assert
        (await IntegrationHelper.CountPendingOutboxMessages()).Should().Be(0);
    }
}