using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Application;

public interface ILanguageRepository
{
    Task CreateAsync(Language language, CancellationToken token);
    Task<Language?> GetAsync(LanguageId languageId, CancellationToken token);
    Task<Language?> GetAsync(ProjectId projectId, LanguageCode code, CancellationToken token);
}