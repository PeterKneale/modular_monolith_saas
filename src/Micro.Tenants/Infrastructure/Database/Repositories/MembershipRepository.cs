using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Domain.Memberships;
using Microsoft.EntityFrameworkCore;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class MembershipRepository(Db db) : IMembershipRepository
{
    public async Task CreateAsync(Membership membership, CancellationToken token)
    {
        await db.Memberships.AddAsync(membership, token);
    }

    public async Task<IEnumerable<Membership>> ListByUserAsync(UserId userId, CancellationToken token)
    {
        return await db.Memberships
            .Where(x => x.UserId == userId)
            .Include(x => x.Organisation)
            .Include(x => x.User)
            .ToListAsync(cancellationToken: token);
    }

    public async Task<IEnumerable<Membership>> ListAsync(OrganisationId organisationId, CancellationToken token)
    {
        return await db.Memberships
            .Where(x => x.OrganisationId == organisationId)
            .Include(x => x.Organisation)
            .Include(x => x.User)
            .ToListAsync(cancellationToken: token);
    }
}