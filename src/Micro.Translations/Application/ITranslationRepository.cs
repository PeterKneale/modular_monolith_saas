using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application;

public interface ITranslationRepository
{
    Task CreateAsync(Translation translation, CancellationToken token);
    Task UpdateAsync(Translation translation, CancellationToken token);
    Task<Translation?> GetAsync(TranslationId id, CancellationToken token);
}