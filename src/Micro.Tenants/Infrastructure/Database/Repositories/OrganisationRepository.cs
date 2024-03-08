using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Domain.Organisations;
using Microsoft.EntityFrameworkCore;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class OrganisationRepository(Db db) : IOrganisationRepository
{
    public async Task CreateAsync(Organisation organisation, CancellationToken token)
    {
        await db.Organisations.AddAsync(organisation, token);
    }

    public void Update(Organisation organisation)
    {
        db.Organisations.Update(organisation);
    }

    public async Task<Organisation?> GetAsync(OrganisationId id, CancellationToken token)
    {
        return await db.Organisations
            .Include(x => x.Memberships)
            .Include(x => x.Projects)
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<Organisation?> GetAsync(OrganisationName name, CancellationToken token)
    {
        return await db.Organisations
            .Include(x => x.Memberships)
            .Include(x => x.Projects)
            .SingleOrDefaultAsync(x => x.Name.Equals(name), token);
    }
}