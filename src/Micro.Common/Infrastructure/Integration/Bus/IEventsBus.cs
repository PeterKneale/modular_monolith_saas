namespace Micro.Common.Infrastructure.Integration.Bus;

public interface IEventsBus
{
    Task Publish<T>(T @event, CancellationToken cancellationToken) where T : IIntegrationEvent;

    void Subscribe<T>(IIntegrationEventHandler handler) where T : IIntegrationEvent;

    void StartConsuming();
}