using Micro.Translations.Domain.TermAggregate.Rules;

namespace Micro.Translations.Domain.TermAggregate;

public class Term : BaseEntity
{
    private readonly List<Translation> _translations;

    private Term()
    {
        // EF Core   
    }

    private Term(TermId termId, ProjectId projectId, TermName termName)
    {
        Id = termId;
        ProjectId = projectId;
        Name = termName;
        _translations = new List<Translation>();
    }

    public TermId Id { get; private set; }

    public ProjectId ProjectId { get; private set; }

    public TermName Name { get; private set; }

    public IReadOnlyCollection<Translation> Translations => _translations;

    public static Term Create(TermId termId, ProjectId projectId, TermName name) =>
        new(termId, projectId, name);

    public static Term Create(ProjectId projectId, TermName name) =>
        Create(TermId.Create(), projectId, name);

    public void AddTranslation(Language language, TranslationText text)
    {
        CheckRule(new MustNotAlreadyHaveTranslationForALanguage(this, language));
        var translationId = TranslationId.Create();
        var translation = Translation.Create(translationId, Id, language, text);
        _translations.Add(translation);
    }

    public void UpdateTranslation(Language language, TranslationText text)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, language));
        var translation = _translations.Single(x => x.Language == language);
        translation.UpdateText(text);
    }

    public void RemoveTranslation(Language language)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, language));
        var translation = _translations.Single(x => x.Language == language);
        _translations.Remove(translation);
    }

    public bool HasTranslationFor(Language language)
    {
        return _translations.Any(x => x.Language == language);
    }

    public Translation GetTranslation(Language language)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, language));
        return _translations.Single(x => x.Language.Equals(language));
    }

    public override string ToString() => $"{Name}";
}