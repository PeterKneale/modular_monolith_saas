using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common.Infrastructure.Integration.Bus;

public static class EventBusExtensions
{
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventsBus, InMemoryEventBus>();
        return services;
    }
}