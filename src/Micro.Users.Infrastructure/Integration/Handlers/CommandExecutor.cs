﻿using Micro.Common.Infrastructure.Integration;

namespace Micro.Users.Infrastructure.Integration.Handlers;

internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command) => 
        await ScopedCommandExecutor.Execute(UsersCompositionRoot.BeginLifetimeScope, command);
}