using Dapper;
using Micro.Common.Application;
using Micro.Common.Infrastructure.Database;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Application.Memberships;

public static class ListMemberships
{
    public record Query() : IRequest<IEnumerable<Result>>;

    public record Result(Guid OrganisationId, string OrganisationName, string Role);

    private class Handler(IUserContext context, ConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT m.{OrganisationIdColumn},o.{NameColumn},m.{RoleColumn} " +
                               $"FROM {MembershipsTable} m " +
                               $"INNER JOIN {OrganisationsTable} o ON m.{OrganisationIdColumn} = o.{IdColumn} " +
                               $"WHERE m.{UserIdColumn} = @UserId";
            using var con = connections.CreateConnection();
            var results = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { UserId = context.UserId.Value }, cancellationToken: token));
            var list = new List<Result>();
            while (results.Read())
            {
                var organisationId = results.GetGuid(0);
                var name = results.GetString(1);
                var role = results.GetString(2);
                list.Add(new Result(organisationId, name, role));
            }

            return list;
        }
    }
}