namespace Micro.Translations.Application.Queries;

public static class GetTranslationStatistics
{
    public record Query : IRequest<Results>;

    public record Results(int TotalTerms, IEnumerable<LanguageStatistic> Statistics);

    public record LanguageStatistic(string Code, int Number, int Percentage);

    private class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, Results>
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
            var allLanguages = await AllLanguages(projectId, token);

            var translationsByLanguage = await GetTranslationsByLanguage(projectId, token);

            var list = new List<LanguageStatistic>();

            // Iterate through all languages, not just those with translations
            foreach (var language in allLanguages)
            {
                // Check if the language has any translations, otherwise set to 0
                translationsByLanguage.TryGetValue(language, out var count);

                // Add the language statistic with the count (0 if no translations)
                var percentage = totalTerms == 0 ? 0 : count * 100 / totalTerms;
                var statistic = new LanguageStatistic(language, count, percentage);
                list.Add(statistic);
            }

            return list;
        }

        private async Task<IDictionary<string, int>> GetTranslationsByLanguage(ProjectId projectId, CancellationToken token)
        {
            var sql = "SELECT lang.language_code AS Code, COUNT(t.id) AS Count FROM translate.languages lang LEFT JOIN translate.translations t ON lang.id = t.language_id WHERE lang.project_id = @projectId GROUP BY lang.language_code";
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            var results = await db.QueryAsync(command);
            return results.ToDictionary(x => x.code as string, x => (int)x.count)!;
        }

        private async Task<IEnumerable<string>> AllLanguages(ProjectId projectId, CancellationToken token)
        {
            var sql = "SELECT language_code FROM translate.languages WHERE project_id = @projectId";
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<string>(command);
        }

        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            var sql = "SELECT COUNT(id) FROM translate.terms WHERE project_id = @projectId";
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.ExecuteScalarAsync<int>(command);
        }
    }
}