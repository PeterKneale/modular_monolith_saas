using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Application.Queries;

public static class CountLanguageTranslations
{
    public record Query(Guid LanguageId) : IRequest<int>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
        }
    }

    private class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = LanguageId.Create(query.LanguageId);
            var sql = """
                      SELECT COUNT(t.id) AS translation_count
                      FROM translations AS t
                      JOIN terms AS term ON t.term_id = term.id
                      JOIN languages AS lang ON t.language_id = lang.id
                      WHERE lang.id =@languageId
                        AND lang.project_id = @projectId;
                      """;
            var command = new CommandDefinition(sql, new { projectId, languageId }, cancellationToken: token);
            return await db.ExecuteScalarAsync<int>(command);
        }
    }
}