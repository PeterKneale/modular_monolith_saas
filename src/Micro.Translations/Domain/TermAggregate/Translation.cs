namespace Micro.Translations.Domain.TermAggregate;

public class Translation : BaseEntity
{
    private Translation()
    {
        // ef core
    }

    private Translation(TranslationId id, TermId termId, Language language, TranslationText text)
    {
        Id = id;
        TermId = termId;
        Language = language;
        Text = text;
    }

    public TranslationId Id { get; private init; } = null!;

    public TermId TermId { get; private init; } = null!;

    public virtual Language Language { get; } = null!;

    public TranslationText Text { get; private set; } = null!;

    public virtual Term Term { get; private set; } = null!;

    public static Translation Create(TranslationId id, TermId termId, Language language, TranslationText text) => new(id, termId, language, text);

    public void UpdateText(TranslationText text)
    {
        Text = text;
    }

    public override string ToString() => $"{Language}: {Text}";
}