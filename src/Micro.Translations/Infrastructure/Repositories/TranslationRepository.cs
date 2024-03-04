using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;
using Micro.Translations.Infrastructure.Database;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Repositories;

internal class TranslationRepository(Db db) : ITranslationRepository
{
    public async Task CreateAsync(Translation translation, CancellationToken token)
    {
        await db.Translations.AddAsync(translation, token);
        // todo remove
        await db.SaveChangesAsync(token);
    }

    public void Update(Translation translation)
    {
        db.Update(translation);
    }

    public async Task<Translation?> GetAsync(TranslationId id, CancellationToken token)
    {
        return await db.Translations
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<Translation?> GetAsync(TermId termId, LanguageId languageId, CancellationToken token)
    {
        return await db.Translations
            .SingleOrDefaultAsync(x => x.TermId == termId && x.LanguageId == languageId, token);
    }

    public async Task<IEnumerable<Translation>> ListAsync(ProjectId projectId, LanguageId languageId, CancellationToken token)
    {
        return await db.Translations
            .Where(x => x.Term.ProjectId == projectId && x.LanguageId == languageId)
            .ToListAsync(cancellationToken: token);
    }
}