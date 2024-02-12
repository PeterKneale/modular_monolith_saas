using Micro.Tenants.Application;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Infrastructure.Services;

public class OrganisationNameCheck(IOrganisationRepository repo) : IOrganisationNameCheck
{
    public async Task<bool> AnyOrganisationUsesNameAsync(OrganisationName name, CancellationToken token)
    {
        return await repo.GetAsync(name, token) != null;
    }

    public async Task<bool> AnyOtherOrganisationUsesNameAsync(OrganisationId id, OrganisationName name, CancellationToken token)
    {
        var organisation = await repo.GetAsync(name, token);
        if (organisation == null)
        {
            // no organisation uses this name
            return false;
        }
        
        // an organisation uses this name, but is it the same organisation?
        var same = organisation.Id.Value == id.Value;
        return !same;
    }
}