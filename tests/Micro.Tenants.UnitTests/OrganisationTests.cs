using FluentAssertions;
using Micro.Common.Domain;
using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.UnitTests;

public class OrganisationTests
{
    private readonly Organisation _organisation;
    private readonly UserId _ownerId = UserId.Create();
    private readonly OrganisationId _id = OrganisationId.Create();
    private readonly OrganisationName _name = OrganisationName.Create("x");

    public OrganisationTests() =>
        _organisation = Organisation.Create(_id, _name, _ownerId);

    [Fact]
    public void Initially_organisation_id_is_set() =>
        _organisation.OrganisationId
            .Should()
            .Be(_id);

    [Fact]
    public void Initially_organisation_name_is_set() =>
        _organisation.Name
            .Should()
            .Be(_name);

    [Fact]
    public void Initially_memberships_is_not_null() =>
        _organisation.Memberships
            .Should()
            .NotBeNull();

    [Fact]
    public void Initially_memberships_is_not_empty() =>
        _organisation.Memberships
            .Should()
            .NotBeEmpty();

    [Fact]
    public void Initially_memberships_contains_a_single_owner() =>
        _organisation.Memberships
            .Should()
            .ContainSingle(x => x.Role.Equals(MembershipRole.Owner));

    [Fact]
    public void Initially_memberships_contains_the_right_owner() =>
        _organisation.Memberships
            .Should()
            .ContainSingle(x => x.UserId.Equals(_ownerId));

    [Fact]
    public void Member_can_be_added()
    {
        // arrange
        var userId = UserId.Create();
        // act
        _organisation.AddMember(userId);
        // assert
        _organisation.Memberships
            .Should()
            .ContainSingle(x => x.UserId.Equals(userId));
    }

    [Fact]
    public void Member_cant_be_added_twice()
    {
        // arrange
        var userId = UserId.Create();
        _organisation.AddMember(userId);
        // act
        var act = () => _organisation.AddMember(userId);
        // assert
        act.Should().Throw<BusinessRuleBrokenException>().WithMessage("User is already a member");
    }

    [Fact]
    public void Member_must_exist_to_be_removed()
    {
        // arrange
        var userId = UserId.Create();
        // act
        var act = () => _organisation.RemoveMember(userId);
        // assert
        act.Should().Throw<BusinessRuleBrokenException>().WithMessage("User is not a member");
    }

    [Fact]
    public void Member_can_be_removed()
    {
        // arrange
        var userId = UserId.Create();
        _organisation.AddMember(userId);

        // act
        _organisation.RemoveMember(userId);

        // assert
        _organisation.Memberships
            .Should()
            .NotContain(x => x.UserId.Equals(userId));
    }

    [Fact]
    public void Owner_cant_be_removed_using_remove_member()
    {
        // act
        var act = () => _organisation.RemoveMember(_ownerId);

        // assert
        act.Should()
            .Throw<BusinessRuleBrokenException>()
            .WithMessage("Membership must be for role Member");
    }

    [Fact]
    public void Member_can_be_promoted_to_owner()
    {
        // arrange
        var userId = UserId.Create();
        _organisation.AddMember(userId);

        // act
        _organisation.PromoteMemberToOwner(userId);

        // assert
        _organisation.Memberships
            .Should()
            .Contain(x => x.UserId.Equals(userId) && x.Role.Equals(MembershipRole.Owner));
    }

    [Fact]
    public void Owner_cant_be_promoted_to_owner_again()
    {
        // act
        var action = () => _organisation.PromoteMemberToOwner(_ownerId);

        // assert
        action.Should()
            .Throw<BusinessRuleBrokenException>()
            .WithMessage("Membership must be for role Member");
    }

    [Fact]
    public void Owner_can_be_demoted_to_member()
    {
        // arrange
        var userId = UserId.Create();
        _organisation.AddMember(userId);
        _organisation.PromoteMemberToOwner(userId);

        // act
        _organisation.DemoteOwnerToMember(userId);

        // assert
        _organisation.Memberships
            .Should()
            .Contain(x => x.UserId.Equals(userId) && x.Role.Equals(MembershipRole.Member));
    }

    [Fact]
    public void Member_cant_be_demoted_to_member()
    {
        // arrange
        var userId = UserId.Create();
        _organisation.AddMember(userId);

        // act
        var action = () => _organisation.DemoteOwnerToMember(userId);

        // assert
        action.Should()
            .Throw<BusinessRuleBrokenException>()
            .WithMessage("Membership must be for role Owner");
    }

    [Fact]
    public void Projects_is_not_null() =>
        _organisation.Projects
            .Should()
            .NotBeNull();

    [Fact]
    public void Projects_is_empty() =>
        _organisation.Projects
            .Should()
            .BeEmpty();
}