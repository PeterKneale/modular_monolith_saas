using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Domain.Terms.Rules;

internal class MustNotAlreadyHaveTranslationForALanguage(Term term, LanguageId languageId) : IBusinessRule
{
    public string Message => "A translation already exists for this language";

    public bool IsBroken() => term.HasTranslationFor(languageId);
}