using Micro.Users.Infrastructure.Infrastructure;

namespace Micro.Users.Infrastructure;

public interface IUsersModule : IModule;

public class UsersModule() : BaseModule(CompositionRoot.BeginLifetimeScope), IUsersModule;