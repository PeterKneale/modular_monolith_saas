using FluentAssertions;
using JetBrains.Annotations;
using Micro.Common.Domain;
using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.UnitTests.Domain.Terms;

[TestSubject(typeof(Term))]
public class TermTest
{
    [Fact]
    public void Can_add_translation_to_term()
    {
        // arrange
        var term = CreateTerm();
        var languageId = LanguageId.Create();
        
        // act
        term.AddTranslation(languageId, new TranslationText("text"));
        
        // assert
        term.HasTranslationFor(languageId).Should().BeTrue();
    }

    [Fact]
    public void Can_not_add_translation_to_term_if_one_exists_for_that_language()
    {
        // arrange
        var term = CreateTerm();
        var languageId = LanguageId.Create();
        term.AddTranslation(languageId, new TranslationText("text"));
        
        // act
        var action = ()=> term.AddTranslation(languageId, new TranslationText("text"));
        
        // assert
        action.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("*already exists*");
    }
    
    [Fact]
    public void Can_add_translation_to_term_if_one_exists_for_a_different_language()
    {
        // arrange
        var term = CreateTerm();
        var languageId1 = LanguageId.Create();
        var languageId2 = LanguageId.Create();
        term.AddTranslation(languageId1, new TranslationText("text"));
        
        // act
        term.AddTranslation(languageId2, new TranslationText("text"));
        
        // assert
        term.HasTranslationFor(languageId1).Should().BeTrue();
        term.HasTranslationFor(languageId2).Should().BeTrue();
    }

    private static Term CreateTerm() => new(TermId.Create(), ProjectId.Create(), new TermName("name"));
}