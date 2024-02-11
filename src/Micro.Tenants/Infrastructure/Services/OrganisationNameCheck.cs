using Micro.Tenants.Application;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Infrastructure.Services;

public class OrganisationNameCheck(IOrganisationRepository repo) : IOrganisationNameCheck
{
    public async Task<bool> AnyOrganisationUsesNameAsync(OrganisationName name)
    {
        return await repo.GetAsync(name) != null;
    }

    public async Task<bool> AnyOtherOrganisationUsesNameAsync(OrganisationId id, OrganisationName name)
    {
        var organisation = await repo.GetAsync(name);
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