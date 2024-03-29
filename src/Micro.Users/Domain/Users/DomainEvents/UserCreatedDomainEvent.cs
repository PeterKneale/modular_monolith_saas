namespace Micro.Users.Domain.Users.DomainEvents;

public class UserCreatedDomainEvent(User user) : IDomainEvent
{
    public User User { get; } = user;
}