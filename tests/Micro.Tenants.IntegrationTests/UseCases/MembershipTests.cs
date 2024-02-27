using FluentAssertions;
using Micro.Tenants.Application.Memberships.Queries;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;
using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class MembershipTests
{
    private readonly ServiceFixture _service;

    public MembershipTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Creating_an_organisation_also_creates_membership()
    {
        // arrange
        var organisationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId);
        var name = Guid.NewGuid().ToString()[..10];
        var create = new CreateOrganisation.Command(organisationId, name);

        // act
        await _service.Command(register);
        await _service.Command(create, userId, organisationId);
        
        // assert organisation exists
        var organisations = await _service.Query(new ListMemberships.Query(), userId);
        organisations.Should().ContainSingle(x=>x.OrganisationId == organisationId);
        
        // assert membership exists
        var memberships = await _service.Query(new ListMemberships.Query(), userId);
        memberships.Should().ContainSingle(x=>
            x.OrganisationId == organisationId &&
            x.OrganisationName == name && 
            x.RoleName == MembershipRole.Owner.Name);

        
    }
}