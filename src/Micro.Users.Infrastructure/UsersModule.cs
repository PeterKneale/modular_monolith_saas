namespace Micro.Users.Infrastructure;

public interface IUsersModule : IModule;

[ExcludeFromCodeCoverage]
public class UsersModule() : BaseModule(UsersCompositionRoot.BeginLifetimeScope), IUsersModule;