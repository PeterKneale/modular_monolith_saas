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
        var register = Build.BuildRegisterCommand(organisationId, userId);
        var update = new UpdateOrganisationName.Command("test2");

        // act
        await _service.Exec(x => x.SendCommand(register));
        await _service.Exec(x => x.SendCommand(update), organisationId, userId);

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
        var register1 = Build.BuildRegisterCommand(organisationId1, userId1);
        var register2 = Build.BuildRegisterCommand(organisationId2, userId2);
        var update = new UpdateOrganisationName.Command(register2.Name);

        await _service.Exec(x => x.SendCommand(register1));
        await _service.Exec(x => x.SendCommand(register2));
        Func<Task> action = async () => await _service.Exec(x => x.SendCommand(update), organisationId1, userId1);

        // assert
        await action.Should().ThrowAsync<Exception>().WithMessage("Name already in use");
    }
}