namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TranslationUpdatedDomainEvent(TermId id, TermName name, Language language, TranslationText oldText, TranslationText newText) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
    public Language Language { get; } = language;
    public TranslationText OldText { get; } = oldText;
    public TranslationText NewText { get; } = newText;
}