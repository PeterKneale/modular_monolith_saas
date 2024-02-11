using FluentAssertions;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Users;

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
        var results = await _service.ExecQ(x => x.SendQuery(new ListMemberships.Query()), userId);

        // assert
        results.Should().ContainSingle(x=>x.OrganisationId == organisationId && x.Role == "Owner");
    }
}

[Collection(nameof(ServiceFixtureCollection))]
public class RegistrationTests
{
    private readonly ServiceFixture _service;

    public RegistrationTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Registering_allows_login()
    {
        // arrange
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId);
        var login = new CanAuthenticate.Query(register.Email, register.Password);

        // act
        await _service.Exec(x => x.SendCommand(register));
        var results = await _service.ExecQ(x => x.SendQuery(login));

        // assert
        results.Success.Should().BeTrue();
        results.UserId.Should().Be(userId);
    }
}