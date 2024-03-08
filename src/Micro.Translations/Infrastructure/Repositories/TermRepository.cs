using Micro.Translations.Application;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Repositories;

internal class TermRepository(Db db) : ITermRepository
{
    public async Task CreateAsync(Term term, CancellationToken token)
    {
        await db.AddAsync(term, token);
    }

    public void Update(Term term)
    {
        db.Update(term);
    }

    public async Task<Term?> GetAsync(TermId id, CancellationToken token)
    {
        return await db.Terms
            .Include(x => x.Translations)
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public Task<Term?> GetAsync(ProjectId projectId, TermName name, CancellationToken cancellationToken)
    {
        return db.Terms
            .Include(x => x.Translations)
            .SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Term>> ListAsync(ProjectId projectId, CancellationToken token)
    {
        return await db.Terms
            .Include(x => x.Translations)
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.Name)
            .ToListAsync(token);
    }
}