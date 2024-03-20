namespace Micro.Common.Infrastructure.Integration.Inbox;

public interface IInboxRepository
{
    Task CreateAsync(InboxMessage message);
    void Update(InboxMessage message);
    Task<List<InboxMessage>> ListPending(CancellationToken token);
}