using Microsoft.Extensions.DependencyInjection;

namespace Micro.Tenants.Infrastructure;

internal static class CompositionRoot
{
    private static IServiceProvider? _provider;

    public static void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static IServiceScope BeginLifetimeScope()
    {
        return _provider?.CreateScope() ?? throw new Exception("Service provider not set.");
    }
}