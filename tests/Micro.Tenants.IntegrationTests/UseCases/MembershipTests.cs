using FluentAssertions;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Application.Organisations;

namespace Micro.Tenants.IntegrationTests;

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
        var name1 = Guid.NewGuid().ToString()[..10];
        var create = new CreateOrganisation.Command(organisationId, name1);

        // act
        await _service.Exec(x => x.SendCommand(register));
        await _service.Exec(x => x.SendCommand(create), userId, organisationId);
        var results = await _service.ExecQ(x => x.SendQuery(new ListOrganisations.Query()), userId);

        // assert
        results.Should().ContainSingle(x=>x.OrganisationId == organisationId);
    }
}