namespace Micro.Common.Infrastructure.Integration.Bus;

public class InMemoryEventBus(ILogger<InMemoryEventBus> logs) : IEventsBus
{
    private readonly IDictionary<string, List<IIntegrationEventHandler>> _subscriptions =
        new Dictionary<string, List<IIntegrationEventHandler>>();

    public Task Publish<T>(T @event, CancellationToken cancellationToken) where T : IntegrationEvent
    {
        var eventType = GetEventType(@event);
        logs.LogDebug("Publishing {EventType} to bus", eventType);

        var handlers = GetHandlers(eventType);
        if (handlers.Count == 0)
        {
            logs.LogDebug("No subscribers for {EventType}", eventType);
            return Task.CompletedTask;
        }
        
        foreach (var handler in handlers)
        {
            logs.LogDebug($"Subscriber handling integration event: {handler.GetType().AssemblyQualifiedName}");
            handler.Handle(@event);
        }

        return Task.CompletedTask;
    }

    public void Subscribe<T>(IIntegrationEventHandler handler) where T : IntegrationEvent
    {
        var eventType = GetEventType<T>();
        if (!_subscriptions.TryGetValue(eventType, out var handlers))
        {
            _subscriptions.Add(eventType, [handler]);
        }
        else
        {
            handlers.Add(handler);
        }
    }

    private List<IIntegrationEventHandler> GetHandlers(string key)
    {
        return _subscriptions.TryGetValue(key, out var handlers) 
            ? handlers 
            : [];
    }
    
    private static string GetEventType<T>() where T : IntegrationEvent =>
        typeof(T).FullName!;
    
    private static string GetEventType<T>(T @event) where T : IntegrationEvent => 
        @event.GetType().FullName!;


    public void StartConsuming()
    {
        throw new NotImplementedException();
    }
}