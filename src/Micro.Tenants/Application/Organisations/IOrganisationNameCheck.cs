using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations;

public interface IOrganisationNameCheck
{
    Task<bool> AnyOrganisationUsesNameAsync(OrganisationName name, CancellationToken token);

    Task<bool> AnyOtherOrganisationUsesNameAsync(OrganisationId id, OrganisationName name, CancellationToken token);
}