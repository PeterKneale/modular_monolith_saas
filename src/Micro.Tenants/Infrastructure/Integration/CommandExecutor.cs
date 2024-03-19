namespace Micro.Tenants.Infrastructure.Integration;

internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IMediator>>();
        logger.LogInformation($"Executing command {command.GetType().Name} in scope");
        await dispatcher.Send(command);
    }
}