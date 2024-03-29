using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;
using Micro.Translations.IntegrationEvents;

namespace Micro.Translations.IntegrationTests.Infrastructure.Integration;

[TestSubject(typeof(ProcessInboxCommand))]
[Collection(nameof(ServiceFixtureCollection))]
public class ProcessOutboxCommandTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Outbox_can_be_processed()
    {
        // arrange
        var termId = Guid.NewGuid();
        var name = "X";
        await IntegrationHelper.PushMessageIntoOutbox(new TermChanged(termId, name));

        // act
        await Service.Command(new ProcessOutboxCommand());

        // assert
        
        // TODO: how to assert,
    }
}