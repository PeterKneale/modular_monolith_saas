using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TranslationUpdatedDomainEvent(TermId id, TermName name, LanguageId languageId, TranslationText oldText, TranslationText newText) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
    public LanguageId LanguageId { get; } = languageId;
    public TranslationText OldText { get; } = oldText;
    public TranslationText NewText { get; } = newText;
}