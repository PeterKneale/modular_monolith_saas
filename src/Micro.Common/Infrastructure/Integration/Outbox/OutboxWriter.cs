namespace Micro.Common.Infrastructure.Integration.Outbox;

public class OutboxWriter(IDbSetOutbox set)
{
    public async Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken token) =>
        await set.Outbox.AddAsync(OutboxMessage.CreateFrom(integrationEvent), token);
}