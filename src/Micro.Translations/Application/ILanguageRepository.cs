using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application;

public interface ILanguageRepository
{
    Task CreateAsync(Language language, CancellationToken token);
    Task<Language?> GetAsync(LanguageId id, CancellationToken token);
    Task<Language?> GetAsync(ProjectId projectId, string code, CancellationToken cancellationToken);
    Task<IEnumerable<Language>> ListAsync(ProjectId projectId, CancellationToken token);
}