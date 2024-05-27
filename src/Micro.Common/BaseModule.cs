using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common;

public class BaseModule
{
    private readonly Func<IServiceScope> _scopeFactory;

    protected BaseModule(Func<IServiceScope> scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SendCommand(IRequest command)
    {
        using var scope = _scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
    {
        using var scope = _scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query);
    }

    public async Task PublishNotification(INotification notification)
    {
        using var scope = _scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification);
    }
}