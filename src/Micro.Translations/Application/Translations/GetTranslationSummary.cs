﻿using Dapper;
using Micro.Common.Infrastructure.Database;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations;

public static class GetTranslationSummary
{
    public record Query(Guid AppId) : IRequest<Result>;

    public record LanguageResult(string Language, int Percentage);

    public record Result(int TotalTerms, int TotalTranslations, IEnumerable<LanguageResult> Languages);

    private class Handler(ConnectionFactory connections) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var totalTerms = await CountTerms(query.AppId, token);
            var totalTranslations = await CountTranslations(query.AppId, token);
            var translationsByLanguage = await CountTranslationsByLanguage(query.AppId, token);
            var languages = translationsByLanguage.Select(x => new LanguageResult(x.Key, x.Value * 100 / totalTerms));
            return new Result(totalTerms, totalTranslations, languages);
        }

        private async Task<int> CountTerms(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE app_id = @AppId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId }, cancellationToken: token));
        }

        private async Task<int> CountTranslations(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TranslationsTable} " +
                      $"JOIN {TermsTable} ON {TranslationsTable}.term_id = terms.id " +
                      "WHERE app_id = @AppId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId }, cancellationToken: token));
        }

        private async Task<IDictionary<string, int>> CountTranslationsByLanguage(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT language_code, COUNT(1) FROM {TermsTable} " +
                      $"INNER JOIN {TranslationsTable} t ON {TermsTable}.id = t.term_id " +
                      $"WHERE {TermsTable}.app_id = @AppId " +
                      $"GROUP BY language_code";

            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { AppId = appId }, cancellationToken: token));
            var result = new Dictionary<string, int>();
            while (reader.Read())
            {
                var language = reader.GetString(0);
                var count = reader.GetInt32(1);
                result.Add(language, count);
            }

            return result;
        }
    }
}