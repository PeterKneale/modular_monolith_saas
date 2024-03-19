namespace Micro.Common.Infrastructure.Integration;

public interface IIntegrationEventHandler
{
    Task Handle(IIntegrationEvent @event);
}