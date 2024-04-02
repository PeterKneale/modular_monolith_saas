using Micro.Common;
using Micro.Users.Infrastructure;

namespace Micro.Users;

public interface IUsersModule : IModule;

public class UsersModule() : BaseModule(CompositionRoot.BeginLifetimeScope), IUsersModule;