namespace Micro.Translations.Domain.TermAggregate.Rules;

internal class MustNotAlreadyHaveTranslationForALanguage(Term term, Language language) : IBusinessRule
{
    public string Message => "A translation already exists for this language";

    public bool IsBroken() => term.HasTranslationFor(language);
}