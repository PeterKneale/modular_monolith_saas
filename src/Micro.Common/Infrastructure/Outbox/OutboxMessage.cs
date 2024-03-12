namespace Micro.Common.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
}

public class IntegrationEvent
{
}