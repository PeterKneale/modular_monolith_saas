using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Domain.Memberships;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Database;

internal class MembershipRepository(ConnectionFactory connections) : IMembershipRepository
{
    public async Task CreateAsync(Membership membership)
    {
        const string sql = $"INSERT INTO {MembershipsTable} (id, organisation_id, user_id, role) " +
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
}