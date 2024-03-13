using JetBrains.Annotations;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Common.Infrastructure.Integration.Outbox;
using Moq;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Micro.Common.UnitTests.Infrastructure.Integration.Outbox;

[TestSubject(typeof(OutboxMessagePublisher))]
public class OutboxMessagePublisherTest(ITestOutputHelper output)
{
    [Fact]
    public async Task Outbox_messages_can_be_published_to_event_bus()
    {
        // arrange
        var logs = output.ToLogger<OutboxMessagePublisher>();
        var bus = new Mock<IEventsBus>();
       
        // act
        var sut = new OutboxMessagePublisher(bus.Object, logs);
        var integrationEvent = new TestIntegrationEvent { TestId = Guid.NewGuid() };
        var message = OutboxMessage.CreateFrom(integrationEvent);

        await sut.PublishToBus(message, CancellationToken.None);

        // assert
        bus.Verify(x => x.Publish(It.IsAny<IntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    public class TestIntegrationEvent : IntegrationEvent
    {
        public Guid TestId { get; set; }
    }
}