namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TranslationAddedDomainEvent(TermId id, TermName name, Language language, TranslationText text) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
    public Language Language { get; } = language;
    public TranslationText Text { get; } = text;
}