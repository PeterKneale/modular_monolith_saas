using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application;

public interface ITranslationRepository
{
    Task CreateAsync(Translation translation, CancellationToken token);
    void Update(Translation translation);
    Task<Translation?> GetAsync(TranslationId id, CancellationToken token);
    Task<Translation?> GetAsync(TermId termId, LanguageId languageId, CancellationToken token);
    Task<IEnumerable<Translation>> ListAsync(ProjectId projectId, LanguageId languageId, CancellationToken token);
}