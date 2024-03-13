namespace Micro.Tenants.Domain.Users.DomainEvents;

public class UserCreatedDomainEvent(UserId id, UserName name) : IDomainEvent
{
    public UserId Id { get; } = id;
    public UserName Name { get; } = name;
}