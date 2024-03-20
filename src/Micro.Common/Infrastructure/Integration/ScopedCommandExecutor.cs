using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common.Infrastructure.Integration;

public static class ScopedCommandExecutor
{
    public static async Task Execute(Func<IServiceScope> createScope, IRequest command)
    {
        using var scope = createScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IMediator>>();
        logger.LogInformation($"Executing command {command.GetType().Name} in scope");
        await mediator.Send(command);
    }
}