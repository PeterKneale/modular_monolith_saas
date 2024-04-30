using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Application.Queries;

public static class ListTranslations
{
    public record Query(Guid LanguageId) : IRequest<Results>;

    public record Results(int TotalTerms, int TotalTranslations, Guid LanguageId, string LanguageCode, IEnumerable<Result> Translations);

    public record Result(Guid TermId, string TermName, Guid? TranslationId, string? TranslationText);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
        }
    }

    private class Handler(IDbConnection connection, IExecutionContext context) : IRequestHandler<Query, Results>
    {
        public async Task<Results> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = LanguageId.Create(query.LanguageId);
            var language = await GetLanguage(languageId, token);
            var totalTerms = await CountTerms(projectId, token);
            var totalTranslations = await CountTranslations(projectId, languageId, token);
            var translations = await ListTranslations(projectId, languageId, token);
            return new Results(totalTerms, totalTranslations, languageId, language, translations);
        }

        private async Task<string> GetLanguage(LanguageId languageId, CancellationToken token)
        {
            const string sql = "select language_code from translate.languages where id = @languageId";

            var command = new CommandDefinition(sql, new
            {
                languageId = languageId.Value
            }, cancellationToken: token);

            return await connection.QuerySingleAsync<string>(command);
        }

        private async Task<IEnumerable<Result>> ListTranslations(ProjectId projectId, LanguageId languageId, CancellationToken token)
        {
            var sql = """
                      SELECT t.id as TermId, t.name as TermName, tr.id as TranslationId, tr.text as TranslationText
                      FROM translate.terms t
                      LEFT JOIN translate.translations tr ON t.id = tr.term_id AND tr.language_id = @languageId
                      WHERE t.project_id = @projectId
                      ORDER BY t.name
                      """;
            var command = new CommandDefinition(sql, new { projectId, languageId }, cancellationToken: token);
            return await connection.QueryAsync<Result>(command);
        }

        private async Task<int> CountTranslations(ProjectId projectId, LanguageId languageId, CancellationToken token)
        {
            var sql = """
                      SELECT COUNT(*) from translate.translations
                      WHERE language_id = @languageId AND term_id IN (
                          SELECT id from translate.terms
                          WHERE project_id = @projectId                     )
                      """;
            var command = new CommandDefinition(sql, new { projectId, languageId }, cancellationToken: token);
            return await connection.ExecuteScalarAsync<int>(command);
        }

        private async Task<int> CountTerms(ProjectId projectId, CancellationToken token)
        {
            var sql = "SELECT COUNT(*) from translate.terms WHERE project_id = @projectId";
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await connection.ExecuteScalarAsync<int>(command);
        }
    }
}