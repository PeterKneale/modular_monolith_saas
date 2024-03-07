using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Domain.Translations;

public class Translation
{
    private Translation()
    {
    }

    public Translation(TranslationId id, TermId termId, Language langauge, TranslationText text)
    {
        Id = id;
        TermId = termId;
        Langauge = langauge;
        Text = text;
    }

    public TranslationId Id { get; private init; }

    public TermId TermId { get; private init; }

    public virtual Language Langauge { get; private init; } = null!;

    public TranslationText Text { get; private set; }

    public virtual Term Term { get; private set; } = null!;

    public void UpdateText(TranslationText translationText)
    {
        Text = translationText;
    }
}