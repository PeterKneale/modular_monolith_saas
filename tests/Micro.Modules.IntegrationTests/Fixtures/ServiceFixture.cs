using MartinCostello.Logging.XUnit;
using MediatR;
using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.IntegrationTests.Common;
using Micro.Tenants;
using Micro.Translations;
using Micro.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ExecutionContext = Micro.Common.Infrastructure.Context.ExecutionContext;

namespace Micro.Modules.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor, IAsyncLifetime
{
    private IModule _tenants = null!;
    private IModule _translations = null!;
    private IModule _users = null!;
    private SettableExecutionContextAccessor _accessor = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
        
        var services = new ServiceCollection()
            .AddTestLogging(this)
            .AddInMemoryEventBus()
            .BuildServiceProvider();
        
        var bus = services.GetRequiredService<IEventsBus>();
        var logs = services.GetRequiredService<ILoggerFactory>();

        _accessor = new SettableExecutionContextAccessor();
        _tenants = new TenantsModule();
        _translations = new TranslationModule();
        _users = new UsersModule();

        await UsersModuleStartup.Start(_accessor, configuration, bus, logs, resetDb:true, enableScheduler:false);
        await TenantsModuleStartup.Start(_accessor, configuration, bus, logs, resetDb:true, enableScheduler:false);
        await TranslationModuleStartup.Start(_accessor, configuration, bus, logs, resetDb:true, enableScheduler:false);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public async Task ExecuteTenants(Func<IModule, Task> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        await action(_tenants);
    }
    
    public async Task ExecuteTranslations(Func<IModule, Task> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        await action(_tenants);
    }

    public async Task CommandTenants(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        await _tenants.SendCommand(command);
    }
    
    public async Task CommandUsers(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        await _users.SendCommand(command);
    }
    
    public async Task CommandTranslations(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        await _translations.SendCommand(command);
    }
    
    public async Task<T> QueryTenants<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        return await _tenants.SendQuery(query);
    }
    
    public async Task<T> QueryTranslations<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext =  new ExecutionContext(userId, organisationId, projectId);
        return await _translations.SendQuery(query);
    }
    
    public async Task PublishTenants(IIntegrationEvent integrationEvent)
    {
        await _tenants.PublishNotification(integrationEvent);
    }
    
    public async Task PublishTranslations(IIntegrationEvent integrationEvent)
    {
        await _translations.PublishNotification(integrationEvent);
    }

}