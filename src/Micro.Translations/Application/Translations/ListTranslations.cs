using System.Data;
using Dapper;
using Micro.Common.Infrastructure.Database;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations;

public static class ListTranslations
{
    public record Query(Guid AppId, string Language) : IRequest<Result>;

    public record LanguageResult(Guid? TranslationId, Guid TermId, string TermName, string? TranslationText);

    public record Result(int TotalTerms, int TotalTranslations, IEnumerable<LanguageResult> Languages);

    private class Handler(ConnectionFactory connections) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var totalTerms = await CountTerms(query.AppId, token);
            var totalTranslations = await CountTranslations(query.AppId, query.Language, token);
            var translationsByLanguage = await ListTranslationsByLanguage(query.AppId, query.Language, token);
            var languages = translationsByLanguage.Select(x => new LanguageResult(x.Key.TranslationId, x.Key.TermId, x.Key.TermName, x.Value));
            return new Result(totalTerms, totalTranslations, languages);
        }

        private async Task<int> CountTerms(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE app_id = @AppId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId }, cancellationToken: token));
        }

        private async Task<int> CountTranslations(Guid appId, string language, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TranslationsTable} " +
                      $"JOIN {TermsTable} on {TranslationsTable}.term_id = {TermsTable}.id " +
                      "WHERE app_id = @appId " +
                      "AND language_code = @language";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId, language }, cancellationToken: token));
        }

        private async Task<IDictionary<(Guid? TranslationId, Guid TermId, string TermName), string?>> ListTranslationsByLanguage(Guid appId, string language, CancellationToken token)
        {
            var sql = $"SELECT tr.id, t.id, t.name, tr.text " +
                      $"FROM {TermsTable} t " +
                      $"LEFT JOIN {TranslationsTable} tr ON t.id = tr.term_id AND tr.language_code = @language " +
                      $"WHERE t.app_id = @appId";

            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { appId, language }, cancellationToken: token));
            var result = new Dictionary<(Guid? TranslationId, Guid TermId, string TermName), string?>();
            while (reader.Read())
            {
                Guid? translationId = reader.IsDBNull(0) ? null : reader.GetGuid(0);
                var termId = reader.GetGuid(1);
                var termName = reader.GetString(2);
                var translationText = reader.IsDBNull(3) ? null : reader.GetString(3);
                result.Add(new ValueTuple<Guid?, Guid, string>(translationId, termId, termName), translationText);
            }
            return result;
        }
    }
}