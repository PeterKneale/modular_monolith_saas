using Micro.Common.Domain;
using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Domain.TermAggregate.DomainEvents;

namespace Micro.Translations.Domain.UnitTests.TermAggregate;

[TestSubject(typeof(Term))]
public class TermTest
{
    private readonly Term _term;
    private readonly TermId _termId = TermId.Create();
    private readonly ProjectId _projectId = ProjectId.Create();
    private readonly TermName _termName = TermName.Create("name");

    public TermTest()
    {
        _term = Term.Create(_termId, _projectId, _termName);
    }

    [Fact]
    public void Translations_are_empty() => 
        _term.Translations.Should().BeEmpty();

    [Fact]
    public void Created_domain_event_is_raised() => 
        _term.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<TermCreatedDomainEvent>();

    [Fact]
    public void Can_add_translation()
    {
        // arrange
        var language = LanguageId.Create();

        // act
        _term.AddTranslation(language, TranslationText.Create("text"));

        // assert
        _term.HasTranslationFor(language).Should().BeTrue();
    }

    [Fact]
    public void Can_update_translation()
    {
        // arrange
        var language = LanguageId.Create();
        var textOriginal = TranslationText.Create("text");
        var textUpdated = TranslationText.Create("text2");
        _term.AddTranslation(language, textOriginal);

        // act
        _term.UpdateTranslation(language, textUpdated);

        // assert
        _term.GetTranslation(language).Text.Should().BeEquivalentTo(textUpdated);
    }

    [Fact]
    public void Can_not_add_translation_to_term_if_one_exists_for_that_language()
    {
        // arrange
        var language = LanguageId.Create();
        _term.AddTranslation(language, TranslationText.Create("text"));

        // act
        var action = () => _term.AddTranslation(language, TranslationText.Create("text"));

        // assert
        action.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("*already exists*");
    }

    [Fact]
    public void Can_add_translation_to_term_if_one_exists_for_a_different_language()
    {
        // arrange
        var language1 = LanguageId.Create();
        var language2 = LanguageId.Create();

        // act
        _term.AddTranslation(language1, TranslationText.Create("text"));
        _term.AddTranslation(language2, TranslationText.Create("text"));

        // assert
        _term.HasTranslationFor(language1).Should().BeTrue();
        _term.HasTranslationFor(language2).Should().BeTrue();
    }
}