using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Domain.Translations;

public class Translation(TranslationId id, TermId termId, LanguageId languageIdId, TranslationText translationText)
{
    public TranslationId Id { get; } = id;
    public TermId TermId { get; } = termId;
    public LanguageId LanguageId { get; } = languageIdId;
    public TranslationText TranslationText { get; private set; } = translationText;

    public void UpdateText(TranslationText translationText)
    {
        TranslationText = translationText;
    }
}