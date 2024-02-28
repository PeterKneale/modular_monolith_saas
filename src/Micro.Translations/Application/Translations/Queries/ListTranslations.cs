using Micro.Common.Application;
using Micro.Translations.Domain;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations.Queries;

public static class ListTranslations
{
    public record Query(Guid LanguageId) : IRequest<Results>;

    public record Results(int TotalTerms, int TotalTranslations, string LanguageName, string LanguageCode, IEnumerable<Result> Translations);

    public record Result(Guid? TranslationId, Guid TermId, string TermName, string? TranslationText);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
        }
    }

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = new LanguageId(query.LanguageId);
            var language = await GetLanguage(languageId, token);
            var totalTerms = await CountTerms(projectId, token);
            var totalTranslations = await CountTranslations(projectId, languageId, token);
            var translations = await ListTranslations(projectId, languageId, token);
            return new Results(totalTerms, totalTranslations, language.Name, language.Code, translations);
        }

        private async Task<LanguageCode> GetLanguage(LanguageId id, CancellationToken token)
        {
            const string sql = $"SELECT {CodeColumn} FROM {LanguagesTable} WHERE id = @Id";
            using var con = connections.CreateConnection();
            var code = await con.ExecuteScalarAsync<string>(new CommandDefinition(sql, new
            {
                id = id.Value
            }, cancellationToken: token));
            return LanguageCode.FromIsoCode(code!);
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

        private async Task<int> CountTranslations(ProjectId projectId, LanguageId languageId, CancellationToken token)
        {
            const string sql = $"SELECT COUNT(1) FROM {TranslationsTable} " +
                               $"JOIN {TermsTable} on {TermsTable}.{IdColumn} = {TranslationsTable}.{TermIdColumn} " +
                               $"WHERE {ProjectIdColumn} = @ProjectId " +
                               $"AND {TranslationsTable}.{LanguageIdColumn} = @LanguageId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                ProjectId = projectId,
                LanguageId = languageId
            }, cancellationToken: token));
        }

        private async Task<IEnumerable<Result>> ListTranslations(ProjectId projectId, LanguageId languageId, CancellationToken token)
        {
            const string sql = $"SELECT tr.id, t.id, t.name, tr.text " +
                               $"FROM {TermsTable} t " +
                               $"LEFT JOIN {TranslationsTable} tr ON t.id = tr.term_id AND tr.language_id = @LanguageId " +
                               $"WHERE t.project_id = @ProjectId";

            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
            {
                ProjectId = projectId,
                LanguageId = languageId
            }, cancellationToken: token));

            var result = new List<Result>();
            while (reader.Read())
            {
                Guid? translationId = reader.IsDBNull(0) ? null : reader.GetGuid(0);
                var termId = reader.GetGuid(1);
                var termName = reader.GetString(2);
                var translationText = reader.IsDBNull(3) ? null : reader.GetString(3);
                result.Add(new Result(translationId, termId, termName, translationText));
            }

            return result;
        }
    }
}