namespace Micro.Common.Infrastructure.Integration;

public class IntegrationEvent : INotification
{
}

public interface IIntegrationEventHandler
{
    Task Handle(IntegrationEvent @event);
}