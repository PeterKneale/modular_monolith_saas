namespace Micro.Users.Domain.Users.DomainEvents;

public class UserNameChangedDomainEvent(UserId id, Name name) : IDomainEvent
{
    public UserId Id { get; } = id;
    public Name Name { get; } = name;
}