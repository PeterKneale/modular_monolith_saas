using Micro.Common.Infrastructure.Integration.Bus;

namespace Micro.Common.Infrastructure.Integration.Outbox;

public class OutboxMessagePublisher(IEventsBus bus, ILogger<OutboxMessagePublisher> logs)
{
    public async Task PublishToBus(OutboxMessage message, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Processing outbox message: {message.Id} {message.Type} {message.Data}");
        try
        {
            var integrationEvent = OutboxMessage.ToIntegrationEvent(message);
            await bus.Publish(integrationEvent, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}