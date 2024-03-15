using MartinCostello.Logging.XUnit;
using MediatR;
using Micro.Common;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExecutionContext = Micro.Common.Infrastructure.Context.ExecutionContext;

namespace Micro.Tenants.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor, IAsyncLifetime
{
    private IModule _module = null!;
    private SettableExecutionContextAccessor _accessor = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection()
            .AddLogging(x => x.AddXUnit(this))
            .AddInMemoryEventBus()
            .BuildServiceProvider();

        var bus = services.GetRequiredService<IEventsBus>();
        var logs = services.GetRequiredService<ILoggerFactory>();

        _accessor = new SettableExecutionContextAccessor
        {
            ExecutionContext = ExecutionContext.Empty()
        };
        _module = new TenantsModule();

        await TenantsModuleStartup.Start(_accessor, configuration, bus, logs, true);
    }

    public async Task DisposeAsync()
    {
        await TenantsModuleStartup.Stop();
    }
    
    public ITestOutputHelper? OutputHelper { get; set; }

    public async Task Execute(Func<IModule, Task> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext = ExecutionContext.Create(userId, organisationId, projectId);
        await action(_module);
    }
    
    public async Task Command(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext = ExecutionContext.Create(userId, organisationId, projectId);
        await _module.SendCommand(command);
    }

    public async Task<T> Query<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext = ExecutionContext.Create(userId, organisationId, projectId);
        return await _module.SendQuery(query);
    }
    
    public async Task Publish(IntegrationEvent integrationEvent)
    {
        await _module.PublishNotification(integrationEvent);
    }
}