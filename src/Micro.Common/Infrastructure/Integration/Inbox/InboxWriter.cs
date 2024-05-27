namespace Micro.Common.Infrastructure.Integration.Inbox;

public class InboxWriter(IDbSetInbox set)
{
    public async Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken token) => await
        set.Inbox.AddAsync(InboxMessage.CreateFrom(integrationEvent), token);
}