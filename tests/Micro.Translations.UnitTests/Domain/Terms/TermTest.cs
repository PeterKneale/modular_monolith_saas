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
        term.AddTranslation(language, TranslationText.Create("text"));

        // assert
        term.HasTranslationFor(language).Should().BeTrue();
    }

    [Fact]
    public void Can_update_translation()
    {
        // arrange
        var term = CreateTerm();
        var language = Language.EnglishAustralian();
        var textOriginal = TranslationText.Create("text");
        var textUpdated = TranslationText.Create("text2");
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
        term.AddTranslation(language, TranslationText.Create("text"));

        // act
        var action = () => term.AddTranslation(language, TranslationText.Create("text"));

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
        term.AddTranslation(language1, TranslationText.Create("text"));
        term.AddTranslation(language2, TranslationText.Create("text"));

        // assert
        term.HasTranslationFor(language1).Should().BeTrue();
        term.HasTranslationFor(language2).Should().BeTrue();
    }

    private static Term CreateTerm() => Term.Create(TermId.Create(), ProjectId.Create(), TermName.Create("name"));
}