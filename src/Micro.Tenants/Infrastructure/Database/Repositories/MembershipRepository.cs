using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class MembershipRepository(Db db) : IMembershipRepository
{
    public async Task CreateAsync(Membership membership, CancellationToken token)
    {
        await db.Memberships.AddAsync(membership, token);
    }

    public void Update(Membership membership) => db.Memberships.Update(membership);

    public void Delete(Membership membership) => db.Memberships.Remove(membership);

    public async Task<Membership?> Get(OrganisationId organisationId, UserId userId, CancellationToken token)
    {
        return await db.Memberships
            .Include(x => x.Organisation)
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.OrganisationId == organisationId && x.UserId == userId, token);
    }

    public async Task<IEnumerable<Membership>> ListAsync(UserId userId, CancellationToken token)
    {
        return await db.Memberships
            .Where(x => x.UserId == userId)
            .Include(x => x.Organisation)
            .Include(x => x.User)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<Membership>> ListAsync(OrganisationId organisationId, CancellationToken token)
    {
        return await db.Memberships
            .Where(x => x.OrganisationId == organisationId)
            .Include(x => x.Organisation)
            .Include(x => x.User)
            .ToListAsync(token);
    }
}