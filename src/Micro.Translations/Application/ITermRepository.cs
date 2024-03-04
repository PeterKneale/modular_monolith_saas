using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application;

public interface ITermRepository
{
    Task CreateAsync(Term term, CancellationToken token);
    void Update(Term term);
    Task<Term?> GetAsync(TermId id, CancellationToken token);
    Task<IEnumerable<Term>> ListAsync(ProjectId projectId, CancellationToken token);
    Task<Term?> GetAsync(ProjectId projectId, TermName name, CancellationToken cancellationToken);
}