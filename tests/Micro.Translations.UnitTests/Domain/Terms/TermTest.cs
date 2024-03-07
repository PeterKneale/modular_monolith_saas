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
        var term = new Term(TermId.Create(), ProjectId.Create(), new TermName("name"));
        var languageId = LanguageId.Create();
        term.AddTranslation(languageId, new TranslationText("text"));
        term.HasTranslationFor(languageId).Should().BeTrue();
    }
}