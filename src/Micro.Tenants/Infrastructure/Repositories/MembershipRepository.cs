using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Domain.Memberships;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Repositories;

internal class MembershipRepository(ConnectionFactory connections) : IMembershipRepository
{
    public async Task CreateAsync(Membership membership, CancellationToken token)
    {
        const string sql = $"INSERT INTO {MembershipsTable} ({IdColumn}, {OrganisationIdColumn}, {UserIdColumn}, {RoleColumn}) " +
                           "VALUES (@Id, @OrganisationId, @UserId, @Role)";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new Row
        {
            Id = membership.Id,
            OrganisationId = membership.OrganisationId,
            UserId = membership.UserId,
            Role = membership.MembershipRole
        }, cancellationToken: token));
    }

    public async Task<IEnumerable<Membership>> ListByUserAsync(UserId userId, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {OrganisationIdColumn}, {RoleColumn} from {MembershipsTable} " +
                           $"WHERE {UserIdColumn} = @UserId)";
        using var con = connections.CreateConnection();
        var rows = await con.QueryAsync<Row>(new CommandDefinition(sql, new
        {
            UserId = userId.Value
        }, cancellationToken: token));
        return rows.Select(Map);
    }

    public async Task<IEnumerable<Membership>> ListAsync(OrganisationId organisationId, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {UserIdColumn}, {RoleColumn} from {MembershipsTable} " +
                           $"WHERE {OrganisationIdColumn} = @OrganisationId)";
        using var con = connections.CreateConnection();
        var rows = await con.QueryAsync<Row>(new CommandDefinition(sql, new Row
        {
            OrganisationId = organisationId
        }, cancellationToken: token));
        return rows.Select(Map);
    }

    private static Membership Map(Row row) =>
        new(row.Id, row.OrganisationId, row.UserId, row.Role);

    public class Row
    {
        public MembershipId Id { get; init; } = null!;
        public UserId UserId { get; init; } = null!;
        public OrganisationId OrganisationId { get; init; } = null!;
        public MembershipRole Role { get; init; } = null!;
    }
}