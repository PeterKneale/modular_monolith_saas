using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application;

public interface ITermRepository
{
    Task CreateAsync(Term term, CancellationToken token);
    Task UpdateAsync(Term term, CancellationToken token);
    Task<Term?> GetAsync(TermId id, CancellationToken token);
}