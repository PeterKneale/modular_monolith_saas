namespace Micro.Common.Infrastructure.Integration.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ProcessedAt { get; init; } = null;

    public static OutboxMessage CreateFrom<T>(T integrationEvent) where T : IntegrationEvent =>
        new()
        {
            Id = Guid.NewGuid(),
            Type = integrationEvent.GetType().AssemblyQualifiedName!,
            Data = JsonConvert.SerializeObject(integrationEvent),
            CreatedAt = DateTime.UtcNow
        };

    public static IntegrationEvent ToIntegrationEvent(OutboxMessage message)
    {
        var messageType = System.Type.GetType(message.Type);
        if (messageType == null) throw new Exception("Unable to find type: " + messageType);
        return JsonConvert.DeserializeObject(message.Data, messageType) as IntegrationEvent ?? throw new InvalidOperationException();
    }
}