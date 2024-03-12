namespace Micro.Common.Infrastructure.Integration.Inbox;

public interface IInboxRepository
{
    Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token);
}