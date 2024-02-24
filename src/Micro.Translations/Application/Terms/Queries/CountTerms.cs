using Micro.Common.Application;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Terms.Queries;

public static class CountTerms
{
    public record Query : IRequest<int>;

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE {ProjectIdColumn} = @ProjectId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                context.ProjectId
            }, cancellationToken: token));
        }
    }
}