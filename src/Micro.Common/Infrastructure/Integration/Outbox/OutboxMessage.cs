using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Integration.Outbox;

public class OutboxMessage
{
    public Guid Id { get; private init; }
    public string Type { get; private init; } = null!;
    public string Data { get; private init; } = null!;
    public DateTimeOffset CreatedAt { get; private init; }
    public DateTimeOffset? ProcessedAt { get; private set; }

    public void MarkProcessed()
    {
        ProcessedAt = SystemClock.UtcNow;
    }

    public static OutboxMessage CreateFrom<T>(T integrationEvent) where T : IIntegrationEvent =>
        new()
        {
            Id = Guid.NewGuid(),
            Type = integrationEvent.GetType().AssemblyQualifiedName!,
            Data = JsonConvert.SerializeObject(integrationEvent),
            CreatedAt = DateTime.UtcNow
        };

    public static IIntegrationEvent ToIntegrationEvent(OutboxMessage message)
    {
        var messageType = System.Type.GetType(message.Type);
        if (messageType == null) throw new Exception("Unable to find type: " + messageType);
        return JsonConvert.DeserializeObject(message.Data, messageType) as IIntegrationEvent ?? throw new InvalidOperationException();
    }
}