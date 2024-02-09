using Micro.Translations.Domain;

namespace Micro.Translations.UnitTests.Domain;

public class LanguageCodeTest
{
    [Fact]
    public void Name_is_correct() => 
        Assert.Equal("English (Australia)", LanguageCode.FromIsoCode("en-AU").Name);
    
    [Fact]
    public void Code_is_correct() => 
        Assert.Equal("en-AU", LanguageCode.FromIsoCode("en-AU").Code);
}