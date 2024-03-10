using FluentAssertions;
using Micro.Common.Exceptions;
using Micro.Tenants.Application;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;

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
    public async Task Organisation_name_can_be_changed()
    {
        // arrange
        var organisationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var name1 = Guid.NewGuid().ToString()[..10];
        var name2 = Guid.NewGuid().ToString()[..10];
        
        var registerUser = Build.RegisterCommand(userId);
        var createOrganisation = new CreateOrganisation.Command(organisationId, name1);
        var updateOrganisation = new UpdateOrganisationName.Command(name2);

        await _service.Command(registerUser);
        await _service.Command(createOrganisation, userId);
        await _service.Command(updateOrganisation, userId, organisationId);

        // assert
        var organisation = await _service.Query(new GetOrganisationByName.Query(name2));
        organisation.Id.Should().Be(organisationId);
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

        await _service.Command(register1);
        await _service.Command(register2);
        await _service.Command(create1, userId1);
        Func<Task> action = async () => await _service.Command(create2, userId1);

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>();
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
        var update = new UpdateOrganisationName.Command(name2);

        await _service.Command(register1);
        await _service.Command(register2);
        await _service.Command(create1, userId1);
        await _service.Command(create2, userId2);
        Func<Task> action = async () => await _service.Command(update, userId1, organisationId1);

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>();
    }
}