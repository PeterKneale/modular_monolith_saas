namespace Micro.Common.Infrastructure.Integration.Queue;

public interface IQueueRepository
{
    Task CreateAsync(QueueMessage command, CancellationToken token);
    void Update(QueueMessage command);
    Task<List<QueueMessage>> ListPending(CancellationToken token);
}