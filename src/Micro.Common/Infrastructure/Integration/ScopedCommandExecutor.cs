using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common.Infrastructure.Integration;

public static class ScopedCommandExecutor
{
    public static async Task Execute(Func<IServiceScope> createScope, IRequest command)
    {
        using var scope = createScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }
}