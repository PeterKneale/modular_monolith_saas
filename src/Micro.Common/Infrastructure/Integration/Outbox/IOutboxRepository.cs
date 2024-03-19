namespace Micro.Common.Infrastructure.Integration.Outbox;

public interface IOutboxRepository
{
    Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token);
    void Update(OutboxMessage message);
    Task<List<OutboxMessage>> ListPending(CancellationToken token);
}