namespace Micro.Common.Infrastructure.Integration.Inbox;

public interface IInboxRepository
{
    Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token);
}

public class InboxRepository(IDbSetInbox set) : IInboxRepository
{
    public async Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token) => await 
        set.Inbox.AddAsync(InboxMessage.CreateFrom(integrationEvent), token);
}