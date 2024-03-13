namespace Micro.Tenants.Domain.Users.DomainEvents;

public class UserNameChangedDomainEvent(UserId id, UserName name) : IDomainEvent
{
    public UserId Id { get; } = id;
    public UserName Name { get; } = name;
}