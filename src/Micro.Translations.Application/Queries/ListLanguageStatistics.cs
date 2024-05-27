namespace Micro.Translations.Application.Queries;

public static class ListLanguageStatistics
{
    public record Query : IRequest<Results>;

    public record Results(int TotalTerms, IEnumerable<LanguageStatistic> Statistics);

    public record LanguageStatistic(Language Language, int Count, int Percentage);

    public record Language(Guid Id, string Name, string Code);

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
            var languages = await ListAllLanguages(projectId, token);

            var translationsByLanguageCode = await GetTranslationsByLanguage(projectId, token);

            var list = new List<LanguageStatistic>();

            // Iterate through all languages, not just those with translations
            foreach (var language in languages)
            {
                // Check if the language has any translations, otherwise set to 0
                translationsByLanguageCode.TryGetValue(language.Code, out var count);

                // Add the language statistic with the count (0 if no translations)
                var percentage = totalTerms == 0 ? 0 : count * 100 / totalTerms;
                var statistic = new LanguageStatistic(language, count, percentage);
                list.Add(statistic);
            }

            return list;
        }

        private async Task<IDictionary<string, int>> GetTranslationsByLanguage(ProjectId projectId, CancellationToken token)
        {
            var sql = """
                      SELECT code AS Code, COUNT(translations.id) AS Count
                      FROM languages
                          LEFT JOIN translations ON languages.id = translations.language_id
                      WHERE languages.project_id = @projectId GROUP BY languages.code
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            var results = await db.QueryAsync(command);
            return results.ToDictionary(x => x.code as string, x => (int)x.count)!;
        }

        private async Task<IEnumerable<Language>> ListAllLanguages(ProjectId projectId, CancellationToken token)
        {
            var sql = "SELECT id, name, code FROM languages WHERE project_id = @projectId";
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<Language>(command);
        }

        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            var sql = "SELECT COUNT(id) FROM terms WHERE project_id = @projectId";
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.ExecuteScalarAsync<int>(command);
        }
    }
}