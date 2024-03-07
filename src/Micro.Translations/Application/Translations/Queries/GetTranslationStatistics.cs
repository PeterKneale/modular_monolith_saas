using Micro.Translations.Domain.Languages;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Translations.Queries;

public static class GetTranslationStatistics
{
    public record Query : IRequest<Results>;

    public record Results(int TotalTerms, IEnumerable<LanguageStatistic> Statistics);

    public record LanguageStatistic(string Code, string Name, int Number, int Percentage);

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var totalTerms = await CountTerms(projectId, token);
            var list = await CalculateStatistics(projectId, totalTerms, token);

            return new Results(totalTerms, list);
        }

        private async Task<List<LanguageStatistic>> CalculateStatistics(ProjectId projectId, int totalTerms, CancellationToken token)
        {
            // Retrieve all languages associated with the project
            var allLanguages = await db.Translations
                .Where(l => l.Term.ProjectId == projectId)
                .Select(x => x.Langauge)
                .Distinct()
                .ToListAsync(token);

            // Retrieve all translations grouped by language for the project
            var translationsByLanguage = await db.Translations
                .Where(x => x.Term.ProjectId == projectId)
                .GroupBy(x => x.Langauge)
                .ToDictionaryAsync(x => x.Key.Code, x => x.Count(), token);

            var list = new List<LanguageStatistic>();

            // Iterate through all languages, not just those with translations
            foreach (var language in allLanguages)
            {
                // Check if the language has any translations, otherwise set to 0
                translationsByLanguage.TryGetValue(language.Code, out var count);

                // Add the language statistic with the count (0 if no translations)
                var percentage = totalTerms == 0 ? 0 : count * 100 / totalTerms;
                var statistic = new LanguageStatistic(language.Code, language.Name, count, percentage);
                list.Add(statistic);
            }

            return list;
        }
        
        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            return await db.Terms
                .Where(x => x.ProjectId == projectId)
                .AsNoTracking()
                .CountAsync(token);
        }
    }
}