using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.TermAggregate.DomainEvents;

public class TranslationRemovedDomainEvent(TermId id, TermName name, LanguageId languageIdId) : IDomainEvent
{
    public TermId Id { get; } = id;
    public TermName Name { get; } = name;
    public LanguageId LanguageId { get; } = languageIdId;
}