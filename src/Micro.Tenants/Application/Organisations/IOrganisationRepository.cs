using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations;

public interface IOrganisationRepository
{
    Task CreateAsync(Organisation organisation, CancellationToken token);
    Task UpdateAsync(Organisation organisation, CancellationToken token);
    Task<Organisation?> GetAsync(OrganisationId id, CancellationToken token);
    Task<Organisation?> GetAsync(OrganisationName name, CancellationToken token);
}