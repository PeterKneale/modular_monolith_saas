using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Application;

public interface IMembershipRepository
{
    Task CreateAsync(Membership membership);
}