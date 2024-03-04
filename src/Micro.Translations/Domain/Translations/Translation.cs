using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Domain.Translations;

public class Translation
{
    private Translation()
    {
    }

    public Translation(TranslationId id, TermId termId, LanguageId languageId, TranslationText text)
    {
        Id = id;
        TermId = termId;
        LanguageId = languageId;
        Text = text;
    }

    public TranslationId Id { get; private init; }

    public TermId TermId { get; private init; }

    public LanguageId LanguageId { get; private init; }

    public TranslationText Text { get; private set; }

    public virtual Language Language { get; private init; } = null!;

    public virtual Term Term { get; private set; } = null!;

    public void UpdateText(TranslationText translationText)
    {
        Text = translationText;
    }
}