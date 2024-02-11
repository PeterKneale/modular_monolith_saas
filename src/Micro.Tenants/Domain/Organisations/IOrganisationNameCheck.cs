namespace Micro.Tenants.Domain.Organisations;

public interface IOrganisationNameCheck
{
    Task<bool> AnyOrganisationUsesNameAsync(OrganisationName name);

    Task<bool> AnyOtherOrganisationUsesNameAsync(OrganisationId id, OrganisationName name);
}