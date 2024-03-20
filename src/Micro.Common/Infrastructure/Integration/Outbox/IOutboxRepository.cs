namespace Micro.Common.Infrastructure.Integration.Outbox;

public interface IOutboxRepository
{
    Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token);
}

public class OutboxRepository(IOutboxDbSet set) : IOutboxRepository
{
    public async Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token) =>
        await set.Outbox.AddAsync(OutboxMessage.CreateFrom(integrationEvent), token);
}