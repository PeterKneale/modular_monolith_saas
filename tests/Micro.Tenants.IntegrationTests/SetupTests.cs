using FluentAssertions;
using Micro.Tenants.Application.Organisations;

namespace Micro.Tenants.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class SetupTests
{
    private readonly ServiceFixture _service;

    public SetupTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_change_organisation_name()
    {
        // arrange
        var organisationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId);
        var name1 = Guid.NewGuid().ToString()[..10];
        var name2 = Guid.NewGuid().ToString()[..10];
        var create = new CreateOrganisation.Command(organisationId, name1);
        var update = new UpdateOrganisationName.Command(organisationId, name2);

        // act
        await _service.Exec(x => x.SendCommand(register));
        await _service.Exec(x => x.SendCommand(create), userId, organisationId);
        await _service.Exec(x => x.SendCommand(update), userId, organisationId);

        // assert
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
        var create1 = new CreateOrganisation.Command(organisationId1, "X");
        var create2 = new CreateOrganisation.Command(organisationId2, "Y");
        var update = new UpdateOrganisationName.Command(organisationId1, "Y");

        await _service.Exec(x => x.SendCommand(register1));
        await _service.Exec(x => x.SendCommand(register2));
        await _service.Exec(x => x.SendCommand(create1), userId1);
        await _service.Exec(x => x.SendCommand(create2), userId2);
        Func<Task> action = async () => await _service.Exec(x => x.SendCommand(update), userId1, organisationId1);

        // assert
        await action.Should().ThrowAsync<Exception>().WithMessage("Name already in use");
    }
}