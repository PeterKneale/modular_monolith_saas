using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Application.Memberships;

public interface IMembershipRepository
{
    Task CreateAsync(Membership membership);
    Task<IEnumerable<Membership>> ListByUserAsync(UserId userId);
}