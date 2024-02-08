using Micro.Common.Domain;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application;

public interface IOrganisationRepository
{
    Task CreateAsync(Organisation organisation);
    Task UpdateAsync(Organisation organisation);
    Task<Organisation?> GetAsync(OrganisationId id);
    Task<Organisation?> GetAsync(OrganisationName name);
}