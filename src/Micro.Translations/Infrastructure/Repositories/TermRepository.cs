using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Repositories;

internal class TermRepository(Db db) : ITermRepository
{
    public async Task CreateAsync(Term term, CancellationToken token)
    {
        await db.AddAsync(term, token);
        // TODO REMOVE
        await db.SaveChangesAsync(token);
    }

    public void Update(Term term)
    {
        db.Update(term);
    }

    public async Task<Term?> GetAsync(TermId id, CancellationToken token)
    {
        return await db.Terms.SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<IEnumerable<Term>> ListAsync(ProjectId projectId, CancellationToken token)
    {
        return await db.Terms.Where(x => x.ProjectId == projectId).ToListAsync(cancellationToken: token);
    }

    public Task<Term?> GetAsync(ProjectId projectId, TermName name, CancellationToken cancellationToken)
    {
        return db.Terms.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Name == name, cancellationToken);
    }
}