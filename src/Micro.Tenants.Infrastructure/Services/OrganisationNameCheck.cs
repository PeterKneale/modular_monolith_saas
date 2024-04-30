namespace Micro.Tenants.Infrastructure.Services;

public class OrganisationNameCheck(IOrganisationRepository repo) : IOrganisationNameCheck
{
    public async Task<bool> AnyOrganisationUsesNameAsync(OrganisationName name, CancellationToken token) => await repo.GetAsync(name, token) != null;

    public async Task<bool> AnyOtherOrganisationUsesNameAsync(OrganisationId id, OrganisationName name, CancellationToken token)
    {
        var organisation = await repo.GetAsync(name, token);
        if (organisation == null)
            // no organisation uses this name
            return false;

        // an organisation uses this name, but is it the same organisation?
        var same = organisation.OrganisationId.Value == id.Value;
        return !same;
    }
}