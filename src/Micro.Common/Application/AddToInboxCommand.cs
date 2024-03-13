using Micro.Common.Infrastructure.Integration;

namespace Micro.Common.Application;

public class AddToInboxCommand(IntegrationEvent integrationEvent) : IRequest
{
    public IntegrationEvent IntegrationEvent { get; init; } = integrationEvent;
}