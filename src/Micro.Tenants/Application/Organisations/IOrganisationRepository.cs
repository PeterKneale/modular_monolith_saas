using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations;

public interface IOrganisationRepository
{
    Task CreateAsync(Organisation organisation, CancellationToken token);
    void Delete(Organisation organisation);
    void Update(Organisation organisation);
    Task<Organisation?> GetAsync(OrganisationId id, CancellationToken token);
    Task<Organisation?> GetAsync(OrganisationName name, CancellationToken token);
    
}