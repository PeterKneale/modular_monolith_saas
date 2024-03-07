using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms.Rules;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Domain.Terms;

public class Term : BaseEntity
{
    private readonly List<Translation> _translations;

    private Term()
    {
        // EF Core   
    }

    public Term(TermId termId, ProjectId projectId, TermName termName)
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

    public static Term Create(string name, ProjectId projectId)
    {
        var termId = new TermId(Guid.NewGuid());
        var termName = new TermName(name);
        return new Term(termId, projectId, termName);
    }

    public void AddTranslation(LanguageId languageId, TranslationText text)
    {
        CheckRule(new MustNotAlreadyHaveTranslationForALanguage(this, languageId));
        var translationId = TranslationId.Create();
        var translation = new Translation(translationId, Id, languageId, text);
        _translations.Add(translation);
    }

    public void UpdateTranslation(LanguageId languageId, TranslationText text)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, languageId));
        var translation = _translations.Single(x => x.LanguageId == languageId);
        translation.UpdateText(text);
    }
    
    public void RemoveTranslation(LanguageId languageId)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, languageId));
        var translation = _translations.Single(x => x.LanguageId == languageId);
        _translations.Remove(translation);
    }

    public bool HasTranslationFor(LanguageId languageId)
    {
        return _translations.Any(x => x.LanguageId == languageId);
    }

    public Translation GetTranslation(LanguageId languageId)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, languageId));
        return _translations.Single(x => x.LanguageId.Equals(languageId));
    }
}