using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Domain.Terms;

public class Term
{
    private Term()
    {
        // EF Core   
    }

    public Term(TermId termId, ProjectId projectId, TermName termName)
    {
        Id = termId;
        ProjectId = projectId;
        Name = termName;
    }

    public TermId Id { get; private set; }

    public ProjectId ProjectId { get; private set; }

    public TermName Name { get; private set; }

    public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();

    public static Term Create(string name, ProjectId projectId)
    {
        var termId = new TermId(Guid.NewGuid());
        var termName = new TermName(name);
        return new Term(termId, projectId, termName);
    }

    public Translation CreateTranslation(LanguageId languageId, TranslationText text)
    {
        var translationId = new TranslationId(Guid.NewGuid());
        return new Translation(translationId, Id, languageId, text);
    }
}