using Micro.Common.Infrastructure.Integration;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Infrastructure.Infrastructure;
using Micro.Tenants.Infrastructure.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Micro.Tenants.IntegrationTests.Fixtures;

public class BaseTest
{
    protected ServiceFixture Service { get; }

    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        Service = service;
    }

    protected async Task<Guid> GivenUser()
    {
        var userId = Guid.NewGuid();
        await Service.Publish(new UserCreated { UserId = userId, Name = "X" });
        return userId;
    }

    protected async Task<Guid> GivenOrganisation(Guid userId, string? name = null)
    {
        var organisationId = Guid.NewGuid();
        name ??= Guid.NewGuid().ToString()[..10];
        var create = new CreateOrganisation.Command(organisationId, name);
        await Service.Command(create, userId);
        return organisationId;
    }

    protected static async Task<IEnumerable<T>> GetOutboxMessages<T>() where T : IIntegrationEvent
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var messages = await db.Outbox
            .Where(x => x.Type.Contains(typeof(T).FullName!))
            .ToListAsync();
        return messages.Select(x => JsonConvert.DeserializeObject<T>(x.Data))!;
    }
}