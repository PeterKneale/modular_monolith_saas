using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Domain.Memberships;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Repositories;

internal class MembershipRepository(ConnectionFactory connections) : IMembershipRepository
{
    public async Task CreateAsync(Membership membership)
    {
        const string sql = $"INSERT INTO {MembershipsTable} ({IdColumn}, {OrganisationIdColumn}, {UserIdColumn}, {RoleColumn}) " +
                           "VALUES (@Id, @OrganisationId, @UserId, @Role)";
        using var con = connections.CreateConnection();
        var parameters = new
        {
            Id = membership.Id.Value,
            OrganisationId = membership.OrganisationId.Value,
            UserId = membership.UserId.Value,
            Role = membership.MembershipRole.Name
        };
        await con.ExecuteAsync(sql, parameters);
    }
    
    public async Task<IEnumerable<Membership>> ListByUserAsync(UserId userId)
    {
        const string sql = $"SELECT {IdColumn}, {OrganisationIdColumn}, {RoleColumn} from {MembershipsTable} " +
                           $"WHERE {UserIdColumn} = @UserId)";
        using var con = connections.CreateConnection();
        var reader = await con.ExecuteReaderAsync(sql, new
        {
            UserId = userId.Value
        });
        var list = new List<Membership>();
        while (reader.Read())
        {
            var id = new MembershipId(reader.GetGuid(0));
            var organisationId = new OrganisationId(reader.GetGuid(1));
            var role = MembershipRole.FromString(reader.GetString(2));
            var membership = new Membership(id, organisationId, userId, role);
            list.Add(membership);
        }
        return list;
    }
    public async Task<IEnumerable<Membership>> ListAsync(OrganisationId organisationId)
    {
        const string sql = $"SELECT {IdColumn}, {UserIdColumn}, {RoleColumn} from {MembershipsTable} " +
                           $"WHERE {OrganisationIdColumn} = @OrganisationId)";
        using var con = connections.CreateConnection();
        var reader = await con.ExecuteReaderAsync(sql, new
        {
            OrganisationId = organisationId.Value
        });
        var list = new List<Membership>();
        while (reader.Read())
        {
            var id = new MembershipId(reader.GetGuid(0));
            var userId = new UserId(reader.GetGuid(1));
            var role = MembershipRole.FromString(reader.GetString(2));
            var membership = new Membership(id, organisationId, userId, role);
            list.Add(membership);
        }
        return list;
    }
}