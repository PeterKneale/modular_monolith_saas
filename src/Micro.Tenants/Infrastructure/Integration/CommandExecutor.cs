namespace Micro.Tenants.Infrastructure.Integration;

internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }
}