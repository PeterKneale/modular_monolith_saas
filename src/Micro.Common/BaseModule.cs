using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common;

public class BaseModule
{
    private readonly Func<AsyncServiceScope> _scopeFactory;

    protected BaseModule(Func<AsyncServiceScope> scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SendCommand(IRequest command)
    {
        await using var scope = _scopeFactory.Invoke();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BaseModule>>();
        logger.LogDebug("Sending command {Command}", command);
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
    {
        await using var scope = _scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query);
    }

    public async Task PublishNotification(INotification notification)
    {
        await using var scope = _scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification);
    }
}