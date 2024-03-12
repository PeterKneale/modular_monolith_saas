using Micro.Common.Infrastructure.Integration;

namespace Micro.Common.Infrastructure.Inbox;

public interface IInboxRepository
{
    Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token);
}