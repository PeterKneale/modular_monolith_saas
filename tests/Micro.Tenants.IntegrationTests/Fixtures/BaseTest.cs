using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Infrastructure;
using Micro.Tenants.Infrastructure.Database;
using Micro.Tenants.IntegrationEvents;
using Micro.Users.IntegrationEvents;
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

    public static async Task<IEnumerable<T>> GetOutboxMessages<T>() where T:IIntegrationEvent
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var messages = await db.Outbox
            .Where(x => x.Type.Contains(typeof(T).FullName))
            .ToListAsync();
        return messages.Select(x => JsonConvert.DeserializeObject<T>(x.Data))!;
    }

    protected async Task CreateUser(Guid userId)
    {
        await Service.Publish(new UserCreated{UserId = userId, Name = "X"});
    }

    protected async Task<Guid> CreateOrganisation(Guid userId, Guid organisationId, string? name = null)
    {
        name ??= Guid.NewGuid().ToString()[..10];
        var create = new CreateOrganisation.Command(organisationId, name);
        await Service.Command(create, userId);
        return organisationId;
    }

    protected async Task UpdateOrganisationName(string name, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var update = new UpdateOrganisationName.Command(name);
        await Service.Command(update, ctxUserId, ctxOrganisationId);
    }

    protected async Task DeleteOrganisation(Guid ctxUserId, Guid ctxOrganisationId)
    {
        var command = new DeleteOrganisation.Command();
        await Service.Command(command, ctxUserId, ctxOrganisationId);
    }

    protected async Task CreateMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var createMember = new CreateMember.Command(userId);
        await Service.Command(createMember, ctxUserId, ctxOrganisationId);
    }

    protected async Task DeleteMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var deleteMember = new RemoveMember.Command(userId);
        await Service.Command(deleteMember, ctxUserId, ctxOrganisationId);
    }
}