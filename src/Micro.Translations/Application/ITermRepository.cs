using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application;

public interface ITermRepository
{
    Task CreateAsync(Term term);
    Task UpdateAsync(Term term);
    Task<Term?> GetAsync(TermId id);
}