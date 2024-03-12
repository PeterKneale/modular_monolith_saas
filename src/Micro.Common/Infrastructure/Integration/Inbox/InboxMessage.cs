namespace Micro.Common.Infrastructure.Integration.Inbox;

public class InboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
}