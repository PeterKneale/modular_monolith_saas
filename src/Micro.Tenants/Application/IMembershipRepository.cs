using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Application;

public interface IMembershipRepository
{
    Task CreateAsync(Membership membership);
    Task<IEnumerable<Membership>> ListByUserAsync(UserId userId);
}