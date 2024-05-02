namespace Micro.Tenants.Infrastructure.Integration.Handlers;

[ExcludeFromCodeCoverage]
internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command) =>
        await ScopedCommandExecutor.Execute(TenantsCompositionRoot.BeginLifetimeScope, command);
}