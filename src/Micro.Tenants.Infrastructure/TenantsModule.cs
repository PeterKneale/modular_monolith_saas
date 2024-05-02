namespace Micro.Tenants.Infrastructure;

public interface ITenantsModule : IModule;

[ExcludeFromCodeCoverage]
public class TenantsModule() : BaseModule(TenantsCompositionRoot.BeginLifetimeScope), ITenantsModule;