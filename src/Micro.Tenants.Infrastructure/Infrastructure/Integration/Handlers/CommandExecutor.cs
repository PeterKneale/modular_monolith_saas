﻿namespace Micro.Tenants.Infrastructure.Infrastructure.Integration.Handlers;

internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command) =>
        await ScopedCommandExecutor.Execute(CompositionRoot.BeginLifetimeScope, command);
}