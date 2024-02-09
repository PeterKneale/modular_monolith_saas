using Micro.Common.Domain;

namespace Micro.Translations.Domain.Languages;

public class Language(LanguageId id, AppId appId, LanguageCode languageCode)
{
    public LanguageId Id { get; } = id;
    public AppId AppId { get; } = appId;
    public LanguageCode LanguageCode { get; } = languageCode;
}