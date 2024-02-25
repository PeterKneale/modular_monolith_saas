using FluentAssertions;
using Micro.Tenants.Application.Organisations.Commands;

namespace Micro.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class OrganisationTests
{
    private readonly ServiceFixture _service;

    public OrganisationTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Cant_create_organisation_if_name_used()
    {
        // arrange
        var organisationId1 = Guid.NewGuid();
        var organisationId2 = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var register1 = Build.RegisterCommand(userId1);
        var register2 = Build.RegisterCommand(userId2);
        var name = Guid.NewGuid().ToString()[..10];
        var create1 = new CreateOrganisation.Command(organisationId1, name);
        var create2 = new CreateOrganisation.Command(organisationId2, name);

        await _service.Exec(x => x.SendCommand(register1));
        await _service.Exec(x => x.SendCommand(register2));
        await _service.Exec(x => x.SendCommand(create1), userId1);
        Func<Task> action = async () => await _service.Exec(x => x.SendCommand(create2), userId1);

        // assert
        await action.Should().ThrowAsync<Exception>().WithMessage("*already in use*");
    }

    [Fact]
    public async Task Cant_change_organisation_name_if_used()
    {
        // arrange
        var organisationId1 = Guid.NewGuid();
        var organisationId2 = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var register1 = Build.RegisterCommand(userId1);
        var register2 = Build.RegisterCommand(userId2);
        var name1 = Guid.NewGuid().ToString()[..10];
        var name2 = Guid.NewGuid().ToString()[..10];
        var create1 = new CreateOrganisation.Command(organisationId1, name1);
        var create2 = new CreateOrganisation.Command(organisationId2, name2);
        var update = new UpdateOrganisationName.Command(organisationId1, name2);

        await _service.Exec(x => x.SendCommand(register1));
        await _service.Exec(x => x.SendCommand(register2));
        await _service.Exec(x => x.SendCommand(create1), userId1);
        await _service.Exec(x => x.SendCommand(create2), userId2);
        Func<Task> action = async () => await _service.Exec(x => x.SendCommand(update), userId1, organisationId1);

        // assert
        await action.Should().ThrowAsync<Exception>().WithMessage("*already in use*");
    }
}