namespace Micro.Translations.Application.Queries;

public static class ListLanguagesTranslated
{
    public record Query : IRequest<IEnumerable<string>>;


    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<string>>
    {
        public async Task<IEnumerable<string>> Handle(Query query, CancellationToken token)
        {
            // list languages where at least one translation exists by joining languages with translations
            var projectId = context.ProjectId;
            var sql = """
                      SELECT lang.language_code AS Code
                      FROM translate.languages lang
                      JOIN translate.translations t ON lang.id = t.language_id
                      WHERE lang.project_id = @projectId
                      GROUP BY lang.language_code
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<string>(command);
        }
    }
}