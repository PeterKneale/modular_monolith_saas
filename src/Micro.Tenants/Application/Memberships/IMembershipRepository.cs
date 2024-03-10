using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Application.Memberships;

public interface IMembershipRepository
{
    Task CreateAsync(Membership membership, CancellationToken token);
    void Update(Membership membership);
    void Delete(Membership membership);
    Task<Membership?> Get(OrganisationId organisationId, UserId userId, CancellationToken token);
    Task<IEnumerable<Membership>> ListAsync(UserId userId, CancellationToken token);
}