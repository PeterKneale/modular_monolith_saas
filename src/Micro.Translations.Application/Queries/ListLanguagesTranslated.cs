namespace Micro.Translations.Application.Queries;

public static class ListLanguagesTranslated
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name, string Code);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            // list languages where at least one translation exists by joining languages with translations
            var projectId = context.ProjectId;
            var sql = """
                      SELECT id, name, code
                      FROM languages
                      WHERE id IN (
                          SELECT t.language_id
                          FROM translations t
                          join terms t2 on t2.id = t.term_id
                          WHERE t2.project_id = @projectId
                          GROUP BY t.language_id
                      )
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<Result>(command);
        }
    }
}