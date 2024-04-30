using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Domain.UnitTests.TermAggregate;

[TestSubject(typeof(TermName))]
public class TermNameTest
{
    [Fact]
    public void Check_equivalency_of_value_types()
    {
        var value = "x";
        var name1 = TermName.Create(value);
        var name2 = TermName.Create(value);
        Equals(name1, name2).Should().BeTrue();
    }
}