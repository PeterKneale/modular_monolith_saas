using Dapper;
using Micro.Common.Application;
using Micro.Common.Infrastructure.Database;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Application.Projects;

public static class ListProjects
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid ProjectId, string ProjectName);

    private class Handler(IOrganisationExecutionContext executionContext, ConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT {IdColumn},{NameColumn} " +
                               $"FROM {ProjectsTable} m " +
                               $"WHERE {OrganisationIdColumn} = @OrganisationId";
            using var con = connections.CreateConnection();
            var results = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { OrganisationId = executionContext.OrganisationId.Value }, cancellationToken: token));
            var list = new List<Result>();
            while (results.Read())
            {
                var id = results.GetGuid(0);
                var name = results.GetString(1);
                list.Add(new Result(id, name));
            }
            return list;
        }
    }
}