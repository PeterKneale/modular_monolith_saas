using Micro.Users;

namespace Micro.Tenants.Infrastructure;

public interface ITenantsModule : IModule;

public class TenantsModule() : BaseModule(TenantsCompositionRoot.BeginLifetimeScope), ITenantsModule;