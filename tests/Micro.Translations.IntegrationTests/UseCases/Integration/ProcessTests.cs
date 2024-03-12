namespace Micro.Translations.IntegrationTests.UseCases.Integration;

[Collection(nameof(ServiceFixtureCollection))]
public class ProcessTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_issue_ProcessInboxCommand()
    {
        await Service.Command(new ProcessInboxCommand());
    }

    [Fact]
    public async Task Can_issue_ProcessOutboxCommand()
    {
        await Service.Command(new ProcessOutboxCommand());
    }
}