using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate.DomainEvents;
using Micro.Translations.Domain.TermAggregate.Rules;

namespace Micro.Translations.Domain.TermAggregate;

public class Term : BaseEntity
{
    private readonly List<Translation> _translations = null!;

    private Term()
    {
        // EF Core   
    }

    private Term(TermId termId, ProjectId projectId, TermName termName)
    {
        Id = termId;
        ProjectId = projectId;
        Name = termName;
        _translations = [];
        AddDomainEvent(new TermCreatedDomainEvent(termId, termName));
    }

    public TermId Id { get; } = null!;

    public ProjectId ProjectId { get; private set; } = null!;

    public TermName Name { get; private set; } = null!;

    public IReadOnlyCollection<Translation> Translations => _translations;

    public static Term Create(TermId termId, ProjectId projectId, TermName name) =>
        new(termId, projectId, name);

    public static Term Create(ProjectId projectId, TermName name) =>
        Create(TermId.Create(), projectId, name);

    public void UpdateName(TermName name)
    {
        var oldName = Name;
        Name = name;
        AddDomainEvent(new TermNameUpdatedDomainEvent(Id, oldName, name));
    }

    public void AddTranslation(LanguageId languageId, TranslationText text)
    {
        CheckRule(new MustNotAlreadyHaveTranslationForALanguage(this, languageId));
        var translationId = TranslationId.Create();
        var translation = Translation.Create(translationId, Id, languageId, text);
        _translations.Add(translation);
        AddDomainEvent(new TranslationAddedDomainEvent(Id, Name, languageId, text));
    }

    public void UpdateTranslation(LanguageId languageId, TranslationText text)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, languageId));
        var translation = _translations.Single(x => x.LanguageId.Equals(languageId));
        var oldText = translation.Text;
        translation.UpdateText(text);
        AddDomainEvent(new TranslationUpdatedDomainEvent(Id, Name, languageId, oldText, text));
    }

    public void RemoveTranslation(LanguageId languageId)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, languageId));
        var translation = _translations.Single(x => x.LanguageId.Equals(languageId));
        _translations.Remove(translation);
        AddDomainEvent(new TranslationRemovedDomainEvent(Id, Name, languageId));
    }

    public bool HasTranslationFor(LanguageId languageId)
    {
        return _translations.Any(x => x.LanguageId.Equals(languageId));
    }

    public Translation GetTranslation(LanguageId languageId)
    {
        CheckRule(new MustHaveTranslationForALanguage(this, languageId));
        return _translations.Single(x => x.LanguageId.Equals(languageId));
    }

    public override string ToString() => $"{Name}";
}