using Micro.Translations.Application;
using Micro.Translations.Domain.Languages;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Repositories;

internal class LanguageRepository(Db db) : ILanguageRepository
{
    public async Task CreateAsync(Language language, CancellationToken token)
    {
        await db.Languages.AddAsync(language, token);
    }

    public async Task<Language?> GetAsync(LanguageId languageId, CancellationToken token)
    {
        return await db.Languages.SingleOrDefaultAsync(x => x.Id == languageId, token);
    }

    public async Task<Language?> GetAsync(ProjectId projectId, LanguageCode code, CancellationToken token)
    {
        return await db.Languages.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.LanguageCode == code, token);
    }
}