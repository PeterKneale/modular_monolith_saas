namespace Micro.Tenants.IntegrationTests.Infrastructure.Integration;

[Collection(nameof(ServiceFixtureCollection))]
public class ProcessInboxCommandTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
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