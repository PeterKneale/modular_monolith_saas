namespace Micro.Translations.Application.Queries;

public static class ListTerms
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name);

    private class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var sql = """
                      SELECT id, name
                      FROM translate.terms
                      WHERE project_id = @projectId;
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<Result>(command);
        }
    }
}