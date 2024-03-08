﻿namespace Micro.Translations.Domain.TermAggregate;

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

    public static Translation Create(TranslationId id, TermId termId, Language language, TranslationText text)
    {
        return new Translation(id, termId, language, text);
    }

    public TranslationId Id { get; private init; }

    public TermId TermId { get; private init; }

    public virtual Language Language { get; private init; } = null!;

    public TranslationText Text { get; private set; }

    public virtual Term Term { get; private set; } = null!;

    public void UpdateText(TranslationText translationText)
    {
        Text = translationText;
    }

    public override string ToString() => $"{Language}: {Text}";
}