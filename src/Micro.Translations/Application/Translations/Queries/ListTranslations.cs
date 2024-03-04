using Micro.Translations.Domain.Languages;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Translations.Queries;

public static class ListTranslations
{
    public record Query(Guid LanguageId) : IRequest<Results>;

    public record Results(int TotalTerms, int TotalTranslations, string LanguageName, string LanguageCode, IEnumerable<Result> Translations);

    public record Result(Guid? TranslationId, Guid TermId, string TermName, string? TranslationText);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
        }
    }

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = new LanguageId(query.LanguageId);
            var language = await GetLanguage(token, languageId);
            var totalTerms = await CountTerms(token, projectId);
            var totalTranslations = await CountTranslations(token, projectId, languageId);
            var translations = await ListTranslations(projectId, languageId);
            return new Results(totalTerms, totalTranslations, language.LanguageCode.Name, language.LanguageCode.Code, translations);
        }

        private async Task<IEnumerable<Result>> ListTranslations(ProjectId projectId, LanguageId languageId)
        {
            return await db.Terms.Where(term => term.ProjectId == projectId)
                .GroupJoin(db.Translations,
                    term => new { TermId = term.Id, LanguageId = languageId },
                    translation => new { translation.TermId, translation.LanguageId },
                    (term, termTranslations) => new { term, termTranslations })
                .SelectMany(t => t.termTranslations.DefaultIfEmpty(), (t, subTranslation) => new Result(subTranslation != null ? subTranslation.Id.Value : null, t.term.Id.Value, t.term.Name.Value, subTranslation != null ? subTranslation.Text.Value : null))
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<int> CountTranslations(CancellationToken token, ProjectId projectId, LanguageId languageId)
        {
            return await db.Translations
                .AsNoTracking()
                .Where(x => x.Term.ProjectId == projectId && x.LanguageId == languageId)
                .CountAsync(token);
        }

        private async Task<int> CountTerms(CancellationToken token, ProjectId projectId)
        {
            return await db.Terms
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .CountAsync(token);
        }

        private async Task<Language> GetLanguage(CancellationToken token, LanguageId languageId)
        {
            return await db.Languages
                .AsNoTracking()
                .SingleAsync(x => x.Id == languageId, token);
        }
    }
}