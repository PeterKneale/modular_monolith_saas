using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Translations.Infrastructure;

namespace Micro.Translations;

public interface ITranslationModule : IModule
{
}

public class TranslationModule : ITranslationModule
{
    public async Task SendCommand(IRequest command)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query);
    }
}