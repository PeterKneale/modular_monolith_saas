namespace Micro.Translations.Domain.Translations;

public class Translation
{
    public TranslationId Id { get; }
    public TermId TermId { get; }
    public Language Language { get; }
    public Text Text { get; private set; }

    public Translation(TranslationId id, TermId termId, Language language, Text text)
    {
        Id = id;
        TermId = termId;
        Language = language;
        Text = text;
    }    
    
    public void SetText(Text text)
    {
        Text = text;
    }
}