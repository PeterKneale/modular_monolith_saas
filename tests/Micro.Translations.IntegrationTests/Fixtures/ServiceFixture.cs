using MartinCostello.Logging.XUnit;
using MediatR;
using Micro.Common;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.IntegrationTests.Common;
using Micro.Translations.Infrastructure;
using Microsoft.Extensions.Logging;
using ExecutionContext = Micro.Common.Infrastructure.Context.ExecutionContext;

namespace Micro.Translations.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor, IAsyncLifetime
{
    private SettableExecutionContextAccessor _accessor = null!;
    private IModule _module = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection()
            .AddTestLogging(this)
            .AddInMemoryEventBus()
            .BuildServiceProvider();

        var bus = services.GetRequiredService<IEventsBus>();
        var logs = services.GetRequiredService<ILoggerFactory>();

        _accessor = new SettableExecutionContextAccessor();
        _module = new TranslationModule();

        await TranslationModuleStartup.Start(_accessor, configuration, bus, logs, resetDb: true, enableScheduler: false);
    }

    public async Task DisposeAsync()
    {
        await TranslationModuleStartup.Stop();
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

    public async Task Publish(IIntegrationEvent integrationEvent)
    {
        await _module.PublishNotification(integrationEvent);
    }
}