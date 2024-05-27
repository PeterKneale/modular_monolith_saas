namespace Micro.Tenants.Domain.UnitTests.Domain;

public class OrganisationNameTests
{
    [Fact]
    public void Organisation_names_are_equal_if_the_underlying_value_is_the_same()
    {
        var name = "X";
        var name1 = OrganisationName.Create(name);
        var name2 = OrganisationName.Create(name);
        name1.Equals(name2).Should().BeTrue();
        name1.Should().Be(name2);
    }

    [Fact]
    public void Organisation_names_are_not_equal_if_the_underlying_value_is_different()
    {
        var name1 = OrganisationName.Create("x");
        var name2 = OrganisationName.Create("y");
        name1.Equals(name2).Should().BeFalse();
        name1.Should().NotBe(name2);
    }
}