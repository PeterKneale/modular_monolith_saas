namespace Micro.Translations.Domain.TermAggregate;

public class Translation
{
    private Translation()
    {
        // ef core
    }

    public Translation(TranslationId id, TermId termId, Language languageCode, TranslationText text)
    {
        Id = id;
        TermId = termId;
        LanguageCode = languageCode;
        Text = text;
    }

    public TranslationId Id { get; private init; }

    public TermId TermId { get; private init; }

    public virtual Language LanguageCode { get; private init; } = null!;

    public TranslationText Text { get; private set; }

    public virtual Term Term { get; private set; } = null!;

    public void UpdateText(TranslationText translationText)
    {
        Text = translationText;
    }
}