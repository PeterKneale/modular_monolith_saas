namespace Micro.Translations.Domain.Languages;

public class Language
{
    private Language()
    {
        // ef core
    }

    public Language(LanguageId id, ProjectId projectId, LanguageCode languageLanguageCode)
    {
        Id = id;
        ProjectId = projectId;
        LanguageCode = languageLanguageCode;
    }

    public LanguageId Id { get; private set; } = null!;

    public ProjectId ProjectId { get; private set; } = null!;

    public LanguageCode LanguageCode { get; private set; } = null!;
}