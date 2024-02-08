using System.Data;
using Dapper;
using Micro.Common.Infrastructure.Database;

namespace Micro.Translations.Application.Translations;

public static class ListTranslations
{
    public record Query(Guid AppId, string Language) : IRequest<Result>;

    public record LanguageResult(string Term, string? Text);

    public record Result(int TotalTerms, int TotalTranslations, IEnumerable<LanguageResult> Languages);

    private class Handler(ConnectionFactory connections) : IRequestHandler<Query, Result>
    {
        const string schema = "translations";
        
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var totalTerms = await CountTerms(query.AppId, token);
            var totalTranslations = await CountTranslations(query.AppId, query.Language, token);
            var translationsByLanguage = await ListTranslationsByLanguage(query.AppId, query.Language, token);
            var languages = translationsByLanguage.Select(x => new LanguageResult(x.Key, x.Value));
            return new Result(totalTerms, totalTranslations, languages);
        }

        private async Task<int> CountTerms(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {schema}.terms WHERE app_id = @AppId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId }, cancellationToken: token));
        }

        private async Task<int> CountTranslations(Guid appId, string language, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {schema}.translations " +
                      $"JOIN {schema}.terms on {schema}.translations.term_id = terms.id " +
                      "WHERE app_id = @appId " +
                      "AND language_code = @language";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId, language }, cancellationToken: token));
        }

        private async Task<IDictionary<string, string?>> ListTranslationsByLanguage(Guid appId, string language, CancellationToken token)
        {
            var sql = $"SELECT t.name, tr.text " +
                      $"FROM translations.terms t " +
                      $"LEFT JOIN translations.translations tr ON t.id = tr.term_id AND tr.language_code = @language " +
                      $"WHERE t.app_id = @appId";
                      
            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { appId, language }, cancellationToken: token));
            return ToDictionary(reader);
        }

        private static Dictionary<string, string?> ToDictionary(IDataReader reader)
        {
            var result = new Dictionary<string, string?>();
            while (reader.Read())
            {
                var term = reader.GetString(0);
                var text = reader.IsDBNull(1) ? null : reader.GetString(1);
                result.Add(term, text);
            }
            return result;
        }
    }
}