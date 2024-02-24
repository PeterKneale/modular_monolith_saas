﻿using Micro.Common.Application;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations.Queries;

public static class GetTranslationStatistics
{
    public record Query : IRequest<Results>;

    public record Results(int TotalTerms, IEnumerable<Result> Statistics);

    public record Result(Guid LanguageId, int Number, int Percentage);

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context, ILogger<Handler> logs) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var totalTerms = await CountTerms(projectId, token);
            var translationsByLanguage = await CountTranslationsByLanguage(projectId, token);
            var statistics = translationsByLanguage.Select(x => new Result(x.Key, x.Value, totalTerms == 0 ? 0 : x.Value * 100 / totalTerms));
            return new Results(totalTerms, statistics);
        }

        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE {ProjectIdColumn} = @ProjectId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { ProjectId = projectId.Value }, cancellationToken: token));
        }

        private async Task<IDictionary<Guid, int>> CountTranslationsByLanguage(ProjectId projectId, CancellationToken token)
        {
            var sql = $"SELECT {LanguagesTable}.{IdColumn}, COUNT(DISTINCT {TranslationsTable}.{TermIdColumn}) FROM {LanguagesTable} " +
                      $"LEFT JOIN {TranslationsTable} ON {LanguagesTable}.{IdColumn} = {TranslationsTable}.{LanguageIdColumn} " +
                      $"WHERE {LanguagesTable}.{ProjectIdColumn} = @ProjectId " +
                      $"GROUP BY {LanguagesTable}.{IdColumn}";
            
            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { ProjectId = projectId.Value }, cancellationToken: token));
            var result = new Dictionary<Guid, int>();
            while (reader.Read())
            {
                var id = reader.GetGuid(0);
                var count = reader.GetInt32(1);
                result.Add(id, count);
            }

            return result;
        }
    }
}