namespace Micro.Translations.Domain.Languages;

public class Language(LanguageId id, ProjectId projectId, LanguageCode languageCode)
{
    public LanguageId Id { get; } = id;
    public ProjectId ProjectId { get; } = projectId;
    public LanguageCode LanguageCode { get; } = languageCode;
}