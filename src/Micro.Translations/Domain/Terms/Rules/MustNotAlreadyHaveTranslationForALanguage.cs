using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Domain.Terms.Rules;

internal class MustNotAlreadyHaveTranslationForALanguage(Term term, Language languageId) : IBusinessRule
{
    public string Message => "A translation already exists for this language";

    public bool IsBroken() => term.HasTranslationFor(languageId);
}