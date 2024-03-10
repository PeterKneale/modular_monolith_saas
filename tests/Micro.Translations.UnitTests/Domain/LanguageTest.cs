using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.UnitTests.Domain;

public class LanguageTest
{
    [Fact]
    public void Name_is_correct() =>
        Assert.Equal("English (Australia)", Language.FromIsoCode("en-AU").Name);

    [Fact]
    public void Code_is_correct() =>
        Assert.Equal("en-AU", Language.FromIsoCode("en-AU").Code);
}