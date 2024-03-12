namespace Micro.Common.Infrastructure.Integration.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
}