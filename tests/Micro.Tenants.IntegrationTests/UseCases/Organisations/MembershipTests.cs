using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;
using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class MembershipTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Creating_an_organisation_also_creates_owner_membership()
    {
        // arrange
        var userId = await GivenUser();

        // act
        var organisationId = await GivenOrganisation(userId);

        // assert
        await AssertUserHasRole(userId, organisationId, MembershipRole.Owner);
    }

    [Fact]
    public async Task Member_can_be_created()
    {
        // arrange
        var userId1 = await GivenUser();
        var userId2 = await GivenUser();
        var organisationId = await GivenOrganisation(userId1);

        // act
        await CreateMember(userId2, userId1, organisationId);

        // assert
        await AssertUserHasRole(userId2, organisationId, MembershipRole.Member);
    }

    [Fact]
    public async Task Member_can_be_deleted()
    {
        // arrange
        var userId1 = await GivenUser();
        var userId2 = await GivenUser();
        var organisationId = await GivenOrganisation(userId1);

        // act
        await CreateMember(userId2, userId1, organisationId);
        await DeleteMember(userId2, userId1, organisationId);

        // assert
        await AssertUserHasNoRole(userId2);
    }

    [Fact]
    public async Task Member_can_be_promoted()
    {
        // arrange
        var userId1 = await GivenUser();
        var userId2 = await GivenUser();
        var organisationId = await GivenOrganisation(userId1);

        // act
        await CreateMember(userId2, userId1, organisationId);
        await Service.Command(new PromoteMemberToOwner.Command(userId2), userId1, organisationId);

        // assert
        await AssertUserHasRole(userId2, organisationId, MembershipRole.Owner);
    }

    [Fact]
    public async Task Member_can_be_demoted()
    {
        // arrange
        var userId1 = await GivenUser();
        var userId2 = await GivenUser();
        var organisationId = await GivenOrganisation(userId1);

        // act
        await CreateMember(userId2, userId1, organisationId);
        await Service.Command(new PromoteMemberToOwner.Command(userId2), userId1, organisationId);
        await Service.Command(new DemoteOwnerToMember.Command(userId2), userId1, organisationId);

        // assert
        await AssertUserHasRole(userId2, organisationId, MembershipRole.Member);
    }


    private async Task CreateMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var createMember = new CreateMember.Command(userId);
        await Service.Command(createMember, ctxUserId, ctxOrganisationId);
    }

    private async Task DeleteMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var deleteMember = new RemoveMember.Command(userId);
        await Service.Command(deleteMember, ctxUserId, ctxOrganisationId);
    }

    private async Task AssertUserHasRole(Guid userId, Guid organisationId, MembershipRole role)
    {
        var members = await Service.Query(new ListMemberships.Query(), userId);
        members.Should().ContainSingle(x =>
            x.OrganisationId == organisationId &&
            x.RoleName == role.Name);
    }

    private async Task AssertUserHasNoRole(Guid userId2)
    {
        var members = await Service.Query(new ListMemberships.Query(), userId2);
        members.Should().BeEmpty();
    }
}