using Dapper;
using Micro.Common.Infrastructure.Database;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Application.Organisations.Queries;

public static class ListMemberships
{
    public record Query : IRequest<IEnumerable<Result>>;

    public class Validator : AbstractValidator<Query>;

    public record Result(Guid OrganisationId, string OrganisationName, string RoleName);

    private class Handler(IExecutionContext context, ConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT " +
                               $"m.{OrganisationIdColumn}, " +
                               $"o.{NameColumn} as {nameof(Result.OrganisationName)}, " +
                               $"m.{RoleColumn} as {nameof(Result.RoleName)} " +
                               $"FROM {MembershipsTable} m " +
                               $"INNER JOIN {OrganisationsTable} o ON m.{OrganisationIdColumn} = o.{IdColumn} " +
                               $"WHERE m.{UserIdColumn} = @UserId";
            using var con = connections.CreateConnection();
            return await con.QueryAsync<Result>(new CommandDefinition(sql, new
            {
                UserId = context.UserId.Value
            }, cancellationToken: token));
        }
    }
}