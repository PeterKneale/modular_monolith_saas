using FluentAssertions;
using JetBrains.Annotations;
using Micro.Common.Domain;
using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.UnitTests.Domain.Terms;

[TestSubject(typeof(Term))]
public class TermTest
{
    [Fact]
    public void Can_add_translation()
    {
        // arrange
        var term = CreateTerm();
        var language = Language.EnglishAustralian();

        // act
        term.AddTranslation(language, new TranslationText("text"));

        // assert
        term.HasTranslationFor(language).Should().BeTrue();
    }

    [Fact]
    public void Can_update_translation()
    {
        // arrange
        var term = CreateTerm();
        var language = Language.EnglishAustralian();
        var textOriginal = new TranslationText("text");
        var textUpdated = new TranslationText("text2");
        term.AddTranslation(language, textOriginal);

        // act
        term.UpdateTranslation(language, textUpdated);

        // assert
        term.GetTranslation(language).Text.Should().BeEquivalentTo(textUpdated);
    }

    [Fact]
    public void Can_not_add_translation_to_term_if_one_exists_for_that_language()
    {
        // arrange
        var term = CreateTerm();
        var language = Language.EnglishAustralian();
        term.AddTranslation(language, new TranslationText("text"));

        // act
        var action = () => term.AddTranslation(language, new TranslationText("text"));

        // assert
        action.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("*already exists*");
    }

    [Fact]
    public void Can_add_translation_to_term_if_one_exists_for_a_different_language()
    {
        // arrange
        var term = CreateTerm();
        var language1 = Language.EnglishAustralian();
        var language2 = Language.EnglishUnitedKingdom();

        // act
        term.AddTranslation(language1, new TranslationText("text"));
        term.AddTranslation(language2, new TranslationText("text"));

        // assert
        term.HasTranslationFor(language1).Should().BeTrue();
        term.HasTranslationFor(language2).Should().BeTrue();
    }

    private static Term CreateTerm() => new(TermId.Create(), ProjectId.Create(), new TermName("name"));
}