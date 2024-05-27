namespace Micro.Translations.Application.Queries;

public static class CountTerms
{
    public record Query : IRequest<int>;

    private class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var sql = """
                      SELECT COUNT(id)
                      FROM terms
                      WHERE project_id = @projectId;
                      """;

            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.ExecuteScalarAsync<int>(command);
        }
    }
}