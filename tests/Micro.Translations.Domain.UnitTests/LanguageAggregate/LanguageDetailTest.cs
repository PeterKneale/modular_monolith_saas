using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Domain.UnitTests.LanguageAggregate;

public class LanguageDetailTest
{
    [Fact]
    public void Name_is_correct() =>
        LanguageDetail.Create("en-AU").Name.Should().Be("English (Australia)");

    [Fact]
    public void Code_is_correct() =>
        LanguageDetail.Create("en-AU").Code.Should().Be("en-AU");

    [Fact]
    public void LoweCase_is_accepted() =>
        LanguageDetail.Create("en-au").Code.Should().Be("en-AU");
}