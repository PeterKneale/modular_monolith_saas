namespace Micro.Translations.Domain.Translations;

public class Translation(TranslationId id, TermId termId, LanguageCode language, Text text)
{
    public TranslationId Id { get; } = id;
    public TermId TermId { get; } = termId;
    public LanguageCode Language { get; } = language;
    public Text Text { get; private set; } = text;

    public void UpdateText(Text text)
    {
        Text = text;
    }
}