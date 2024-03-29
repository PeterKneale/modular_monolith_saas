using Micro.Tenants.Application.Memberships.Commands;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Domain.Memberships;
using Micro.Users.IntegrationEvents;

namespace Micro.Tenants.IntegrationTests.Fixtures;

public class BaseTest
{
    protected ServiceFixture Service { get; }

    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        Service = service;
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
        var role = MembershipRole.Member.Name;
        var createMember = new CreateMember.Command(userId, role);
        await Service.Command(createMember, ctxUserId, ctxOrganisationId);
    }

    protected async Task DeleteMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var deleteMember = new DeleteMember.Command(userId);
        await Service.Command(deleteMember, ctxUserId, ctxOrganisationId);
    }
}