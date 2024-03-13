using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.IntegrationEvents;

public class UserCreated : IntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}