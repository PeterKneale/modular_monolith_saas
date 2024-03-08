using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Queries;

public static class ListTranslations
{
    public record Query(string LanguageCode) : IRequest<Results>;

    public record Results(int TotalTerms, int TotalTranslations, string LanguageName, string LanguageCode, IEnumerable<Result> Translations);

    public record Result(Guid? TranslationId, Guid TermId, string TermName, string? TranslationText);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageCode).NotEmpty();
        }
    }

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var language = Language.FromIsoCode(query.LanguageCode);
            var totalTerms = await CountTerms(token, projectId);
            var totalTranslations = await CountTranslations(token, projectId, language);
            var translations = await ListTranslations(projectId, language);
            return new Results(totalTerms, totalTranslations, language.Name, language.Code, translations);
        }

        private async Task<IEnumerable<Result>> ListTranslations(ProjectId projectId, Language language)
        {
            return await db.Terms.Where(term => term.ProjectId == projectId)
                .GroupJoin(db.Translations,
                    term => new { TermId = term.Id, Language = language },
                    translation => new { translation.TermId, translation.Language },
                    (term, termTranslations) => new { term, termTranslations })
                .SelectMany(t => t.termTranslations.DefaultIfEmpty(), (t, subTranslation) => new Result(subTranslation != null ? subTranslation.Id.Value : null, t.term.Id.Value, t.term.Name.Value, subTranslation != null ? subTranslation.Text.Value : null))
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<int> CountTranslations(CancellationToken token, ProjectId projectId, Language language)
        {
            return await db.Translations
                .AsNoTracking()
                .Where(x => x.Term.ProjectId == projectId && x.Language == language)
                .CountAsync(token);
        }

        private async Task<int> CountTerms(CancellationToken token, ProjectId projectId)
        {
            return await db.Terms
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .CountAsync(token);
        }
    }
}