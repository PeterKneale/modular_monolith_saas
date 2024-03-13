using MartinCostello.Logging.XUnit;
using MediatR;
using Micro.Common;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Integration.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using ExecutionContext = Micro.Common.Infrastructure.Context.ExecutionContext;

namespace Micro.Tenants.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor
{
    private readonly IModule _module;
    private readonly ExecutionContextAccessor _accessor;

    public ServiceFixture()
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

        _accessor = new ExecutionContextAccessor
        {
            ExecutionContext = ExecutionContext.Empty()
        };
        _module = new TenantsModule();

        TenantsModuleStartup.Start(_accessor, configuration, bus, logs, true);
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public async Task Command(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext = ExecutionContext.Create(userId, organisationId, projectId);
        await _module.SendCommand(command);
        _accessor.ExecutionContext = ExecutionContext.Empty();
    }

    public async Task<T> Query<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        _accessor.ExecutionContext = ExecutionContext.Create(userId, organisationId, projectId);
        var result = await _module.SendQuery(query);
        _accessor.ExecutionContext = ExecutionContext.Empty();
        return result;
    }
}