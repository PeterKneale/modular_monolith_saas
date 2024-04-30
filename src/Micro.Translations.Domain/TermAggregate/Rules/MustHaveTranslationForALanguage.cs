using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.TermAggregate.Rules;

internal class MustHaveTranslationForALanguage(Term term, LanguageId languageId) : IBusinessRule
{
    public string Message => "A translation does not exist for this language";

    public bool IsBroken() => !term.HasTranslationFor(languageId);
}