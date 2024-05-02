using Micro.Common.Infrastructure.Integration;

namespace Micro.Translations.Infrastructure.Integration.Handlers;

[ExcludeFromCodeCoverage]
internal static class CommandExecutor
{
    public static async Task SendCommand(IRequest command) => 
        await ScopedCommandExecutor.Execute(TranslationsCompositionRoot.BeginLifetimeScope, command);
}