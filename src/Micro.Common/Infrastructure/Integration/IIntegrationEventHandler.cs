namespace Micro.Common.Infrastructure.Integration;

public interface IIntegrationEventHandler
{
    Task Handle(IntegrationEvent @event);
}