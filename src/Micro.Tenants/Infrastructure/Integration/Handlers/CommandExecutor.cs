using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.Infrastructure.Integration.Handlers;

internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command) => 
        await ScopedCommandExecutor.Execute(CompositionRoot.BeginLifetimeScope, command);
}