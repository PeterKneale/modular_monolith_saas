namespace Micro.Common.Domain;

[ExcludeFromCodeCoverage]
public abstract class BaseEntity
{
    private readonly List<BaseEvent> _domainEvents = new();

    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken()) throw new BusinessRuleBrokenException(rule.Message);
    }
}