using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.TermAggregate;

public class Translation : BaseEntity
{
    private Translation()
    {
        // ef core
    }

    private Translation(TranslationId id, TermId termId, LanguageId languageId, TranslationText text)
    {
        Id = id;
        TermId = termId;
        LanguageId = languageId;
        Text = text;
    }

    public TranslationId Id { get; private init; } = null!;

    public TermId TermId { get; private init; } = null!;

    public LanguageId LanguageId { get; } = null!;

    public TranslationText Text { get; private set; } = null!;

    public static Translation Create(TranslationId id, TermId termId, LanguageId languageId, TranslationText text) => new(id, termId, languageId, text);

    public void UpdateText(TranslationText text)
    {
        Text = text;
    }

    public override string ToString() => $"{LanguageId}: {Text}";
}