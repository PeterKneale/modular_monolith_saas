namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TermCreatedDomainEvent(TermId id, TermName name) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
}