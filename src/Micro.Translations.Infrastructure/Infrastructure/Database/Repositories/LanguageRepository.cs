using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Infrastructure.Database.Repositories;

internal class LanguageRepository(Db db) : ILanguageRepository
{
    public async Task CreateAsync(Language language, CancellationToken token) =>
        await db.Languages.AddAsync(language, token);

    public async Task<Language?> GetAsync(LanguageId id, CancellationToken token) =>
        await db.Languages
            .SingleOrDefaultAsync(x => x.LanguageId.Equals(id), token);

    public async Task<Language?> GetAsync(ProjectId projectId, string code, CancellationToken cancellationToken) => 
        await db.Languages.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Detail.Code == code, cancellationToken);

    public async Task<IEnumerable<Language>> ListAsync(ProjectId projectId, CancellationToken token) => 
        await db.Languages.Where(x=>x.ProjectId == projectId).ToListAsync(token);
}