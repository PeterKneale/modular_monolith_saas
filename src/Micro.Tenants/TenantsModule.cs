using Micro.Common;
using Micro.Tenants.Infrastructure;
using Micro.Users;

namespace Micro.Tenants;

public interface ITenantsModule : IModule;

public class TenantsModule() : BaseModule(CompositionRoot.BeginLifetimeScope), ITenantsModule;