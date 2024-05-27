namespace Micro.Translations.Domain.LanguageAggregate;

public class Language : BaseEntity
{
    private Language()
    {
        // EF Core   
    }

    private Language(LanguageId languageId, ProjectId projectId, LanguageDetail detail)
    {
        LanguageId = languageId;
        ProjectId = projectId;
        Detail = detail;
    }

    public LanguageId LanguageId { get; }
    public ProjectId ProjectId { get; }
    public LanguageDetail Detail { get; }

    public static Language FromIsoCode(LanguageId languageId, ProjectId projectId, string isoCode)
    {
        var languageCode = LanguageDetail.Create(isoCode);
        return new Language(languageId, projectId, languageCode);
    }
}