namespace Micro.Common.Infrastructure.Outbox;

public interface IOutboxRepository
{
    Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token);
}