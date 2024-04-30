using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.TermAggregate.Rules;

internal class MustNotAlreadyHaveTranslationForALanguage(Term term, LanguageId languageId) : IBusinessRule
{
    public string Message => "A translation already exists for this language";

    public bool IsBroken() => term.HasTranslationFor(languageId);
}