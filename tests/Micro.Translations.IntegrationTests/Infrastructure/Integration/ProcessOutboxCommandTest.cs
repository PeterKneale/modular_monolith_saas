﻿using Micro.Translations.IntegrationEvents;

namespace Micro.Translations.IntegrationTests.Infrastructure.Integration;

[TestSubject(typeof(ProcessInboxCommand))]
[Collection(nameof(ServiceFixtureCollection))]
public class ProcessOutboxCommandTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Outbox_can_be_processed()
    {
        // arrange
        await IntegrationHelper.PurgeOutbox();
        await IntegrationHelper.PushMessageIntoOutbox(new TermChanged(Guid.NewGuid(), "X"));
        (await IntegrationHelper.CountPendingOutboxMessages()).Should().Be(1);

        // act
        await Service.Command(new ProcessOutboxCommand());

        // assert
        (await IntegrationHelper.CountPendingOutboxMessages()).Should().Be(0);
    }
}