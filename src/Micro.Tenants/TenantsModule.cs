using Micro.Tenants.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Tenants;

public class TenantsModule : ITenantsModule
{
    public async Task SendCommand(IRequest command)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query);
    }

    public async Task PublishNotification(INotification notification)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification);
    }
}