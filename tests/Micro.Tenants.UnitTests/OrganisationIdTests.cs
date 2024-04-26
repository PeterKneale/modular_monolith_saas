using FluentAssertions;
using Micro.Common.Domain;

namespace Micro.Tenants.UnitTests;

public class OrganisationIdTests
{
    [Fact]
    public void Organisation_ids_are_equal_if_the_underlying_guid_is_the_same()
    {
        var id = Guid.NewGuid();
        var id1 = OrganisationId.Create(id);
        var id2 = OrganisationId.Create(id);
        id1.Equals(id2).Should().BeTrue();
        id1.Should().Be(id2);
    }
    
    [Fact]
    public void Organisation_ids_are_not_equal_if_the_underlying_guid_is_different()
    {
        var id1 = OrganisationId.Create();
        var id2 = OrganisationId.Create();
        id1.Equals(id2).Should().BeFalse();
        id1.Should().NotBe(id2);
    }
}