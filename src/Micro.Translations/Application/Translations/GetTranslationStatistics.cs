using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Domain;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations;

public static class GetTranslationStatistics
{
    public record Query : IRequest<Results>;

    public record Results(int TotalTerms, int TotalTranslations, IEnumerable<Result> Statistics);

    public record Result(ResultLanguage ResultLanguage, int Percentage);

    public record ResultLanguage(string Name, string Code);

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var totalTerms = await CountTerms(projectId, token);
            var totalTranslations = await CountTranslations(projectId, token);
            var translationsByLanguage = await CountTranslationsByLanguage(projectId, token);
            var statistics = translationsByLanguage.Select(x => new Result(x.Key, x.Value * 100 / totalTerms));
            return new Results(totalTerms, totalTranslations, statistics);
        }

        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE project_id = @ProjectId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { ProjectId = projectId.Value }, cancellationToken: token));
        }

        private async Task<int> CountTranslations(ProjectId projectId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TranslationsTable} " +
                      $"JOIN {TermsTable} ON {TranslationsTable}.term_id = terms.id " +
                      "WHERE project_id = @ProjectId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { ProjectId = projectId.Value }, cancellationToken: token));
        }

        private async Task<IDictionary<ResultLanguage, int>> CountTranslationsByLanguage(ProjectId projectId, CancellationToken token)
        {
            var sql = $"SELECT language_code, COUNT(1) FROM {TermsTable} " +
                      $"INNER JOIN {TranslationsTable} t ON {TermsTable}.id = t.term_id " +
                      $"WHERE {TermsTable}.project_id = @ProjectId " +
                      $"GROUP BY language_code";

            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { ProjectId = projectId.Value }, cancellationToken: token));
            var result = new Dictionary<ResultLanguage, int>();
            while (reader.Read())
            {
                var language = LanguageCode.FromIsoCode(reader.GetString(0));
                var count = reader.GetInt32(1);
                result.Add(new ResultLanguage(language.Name, language.Code), count);
            }

            return result;
        }
    }
}