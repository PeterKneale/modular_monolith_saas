namespace Micro.Users.Messages;

public class UserChanged : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}