using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Integration.Queue;

public class QueueMessage
{
    public Guid Id { get; private init; }
    public string Type { get; private init; } = null!;
    public string Data { get; private init; } = null!;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ProcessedAt { get; private set; }

    public void MarkProcessed()
    {
        ProcessedAt = SystemClock.UtcNow;
    }

    public static QueueMessage CreateFrom<T>(T command) where T : IQueuedCommand =>
        new()
        {
            Id = Guid.NewGuid(),
            Type = command.GetType().AssemblyQualifiedName!,
            Data = JsonConvert.SerializeObject(command),
            CreatedAt = DateTime.UtcNow
        };

    public static IQueuedCommand ToRequest(QueueMessage command)
    {
        var messageType = System.Type.GetType(command.Type);
        if (messageType == null) throw new Exception("Unable to find type: " + messageType);
        return JsonConvert.DeserializeObject(command.Data, messageType) as IQueuedCommand ?? throw new InvalidOperationException();
    }
}