namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TranslationRemovedDomainEvent(TermId id, TermName name, Language language) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
    public Language Language { get; } = language;
}