using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Domain;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations;

public static class ListTranslations
{
    public record Query(string Language) : IRequest<Result>;

    public record LanguageResult(Guid? TranslationId, Guid TermId, string TermName, string? TranslationText);

    public record Result(int TotalTerms, int TotalTranslations, IEnumerable<LanguageResult> Languages);

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var language = LanguageCode.FromIsoCode(query.Language);
            var totalTerms = await CountTerms(projectId, token);
            var totalTranslations = await CountTranslations(projectId, language, token);
            var translations = await ListTranslations(projectId, language, token);
            return new Result(totalTerms, totalTranslations, translations);
        }

        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            const string sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE project_id = @ProjectId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                ProjectId = projectId.Value
            }, cancellationToken: token));
        }

        private async Task<int> CountTranslations(ProjectId projectId, LanguageCode language, CancellationToken token)
        {
            const string sql = $"SELECT COUNT(1) FROM {TranslationsTable} " +
                               $"JOIN {TermsTable} on {TranslationsTable}.term_id = {TermsTable}.id " +
                               "WHERE project_id = @ProjectId " +
                               "AND language_code = @LanguageCode";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                ProjectId = projectId.Value, 
                LanguageCode = language.Code
            }, cancellationToken: token));
        }

        private async Task<IEnumerable<LanguageResult>> ListTranslations(ProjectId projectId, LanguageCode language, CancellationToken token)
        {
            const string sql = $"SELECT tr.id, t.id, t.name, tr.text " +
                               $"FROM {TermsTable} t " +
                               $"LEFT JOIN {TranslationsTable} tr ON t.id = tr.term_id AND tr.language_code = @LanguageCode " +
                               $"WHERE t.project_id = @ProjectId";

            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
            {
                ProjectId = projectId.Value, 
                LanguageCode = language.Code
            }, cancellationToken: token));
            
            var result = new List<LanguageResult>();
            while (reader.Read())
            {
                Guid? translationId = reader.IsDBNull(0) ? null : reader.GetGuid(0);
                var termId = reader.GetGuid(1);
                var termName = reader.GetString(2);
                var translationText = reader.IsDBNull(3) ? null : reader.GetString(3);
                result.Add(new LanguageResult(translationId, termId, termName, translationText));
            }

            return result;
        }
    }
}