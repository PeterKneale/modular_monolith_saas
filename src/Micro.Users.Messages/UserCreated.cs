namespace Micro.Users.Messages;

public class UserCreated : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}