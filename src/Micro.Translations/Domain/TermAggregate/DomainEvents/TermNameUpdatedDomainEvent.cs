namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TermNameUpdatedDomainEvent(TermId id, TermName oldName, TermName newName) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName OldName { get; } = oldName;
    public TermName NewName { get; } = newName;
}