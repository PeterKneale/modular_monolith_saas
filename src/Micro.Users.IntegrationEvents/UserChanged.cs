namespace Micro.Users.IntegrationEvents;

public class UserChanged : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}