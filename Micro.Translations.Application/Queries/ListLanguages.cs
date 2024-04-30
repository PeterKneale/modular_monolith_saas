namespace Micro.Translations.Application.Queries;

public static class ListLanguages
{
    public record Query : IRequest<IEnumerable<string>>;


    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<string>>
    {
        public async Task<IEnumerable<string>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var sql = """
                      SELECT lang.language_code AS Code
                      FROM translate.languages lang
                      WHERE lang.project_id = @projectId
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<string>(command);
        }
    }
}