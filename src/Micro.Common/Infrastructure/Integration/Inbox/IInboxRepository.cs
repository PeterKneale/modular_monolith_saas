namespace Micro.Common.Infrastructure.Integration.Inbox;

public interface IInboxRepository
{
    Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token);
    void Update(InboxMessage message);
    Task<List<InboxMessage>> ListPending(CancellationToken token);
}