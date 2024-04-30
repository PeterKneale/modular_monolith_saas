using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.UnitTests.LanguageAggregate;

public class LanguageIdTest
{
    [Fact]
    public void Check_equivalency()
    {
        var value = Guid.NewGuid();
        var languageId1 = LanguageId.Create(value);
        var languageId2 = LanguageId.Create(value);
        languageId1.Equals(languageId2).Should().BeTrue();
    }
}