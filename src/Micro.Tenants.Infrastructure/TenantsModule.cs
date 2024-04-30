using Micro.Tenants.Infrastructure.Infrastructure;
using Micro.Users;

namespace Micro.Tenants.Infrastructure;

public interface ITenantsModule : IModule;

public class TenantsModule() : BaseModule(CompositionRoot.BeginLifetimeScope), ITenantsModule;