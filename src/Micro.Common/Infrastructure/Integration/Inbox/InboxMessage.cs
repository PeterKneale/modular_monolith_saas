namespace Micro.Common.Infrastructure.Integration.Inbox;

public class InboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
    
    public static InboxMessage CreateFrom<T>(T integrationEvent) where T : IntegrationEvent
    {
        return new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = integrationEvent.GetType().AssemblyQualifiedName!,
            Data = JsonConvert.SerializeObject(integrationEvent)
        };
    }
    
    public static IntegrationEvent ToIntegrationEvent(InboxMessage message)
    {
        var messageType = System.Type.GetType(message.Type);
        if (messageType == null)
        {
            throw new Exception("Unable to find type: " + messageType);
        }
        return JsonConvert.DeserializeObject(message.Data, messageType) as IntegrationEvent ?? throw new InvalidOperationException();
    }
}