namespace Micro.Common.Infrastructure.Integration.Bus;

public interface IEventsBus
{
    Task Publish<T>(T @event, CancellationToken cancellationToken) where T : IntegrationEvent;

    void Subscribe<T>(IIntegrationEventHandler handler) where T : IntegrationEvent;

    void StartConsuming();
}