using Micro.Common.Infrastructure.Integration;

namespace Micro.Common.Application;

public class AddToOutboxCommand : IRequest
{
    public IntegrationEvent IntegrationEvent { get; init; }
}