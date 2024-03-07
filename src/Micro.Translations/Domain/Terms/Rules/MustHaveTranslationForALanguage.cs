using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Domain.Terms.Rules;

internal class MustHaveTranslationForALanguage(Term term, Language language) : IBusinessRule
{
    public string Message => "A translation does not exist for this language";

    public bool IsBroken() => !term.HasTranslationFor(language);
}