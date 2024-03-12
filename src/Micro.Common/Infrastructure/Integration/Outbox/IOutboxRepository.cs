namespace Micro.Common.Infrastructure.Integration.Outbox;

public interface IOutboxRepository
{
    Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token);
}