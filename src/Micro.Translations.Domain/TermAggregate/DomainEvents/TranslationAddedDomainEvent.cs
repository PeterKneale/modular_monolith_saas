using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TranslationAddedDomainEvent(TermId id, TermName name, LanguageId languageId, TranslationText text) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
    public LanguageId Language { get; } = languageId;
    public TranslationText Text { get; } = text;
}