namespace Micro.Users.Infrastructure;

public interface IUsersModule : IModule;

public class UsersModule() : BaseModule(UsersCompositionRoot.BeginLifetimeScope), IUsersModule;