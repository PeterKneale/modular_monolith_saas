using Micro.Common.Infrastructure.Integration;

namespace Micro.Common.Application;

public class AddToInboxCommand : IRequest
{
    public IntegrationEvent IntegrationEvent { get; init; }
}