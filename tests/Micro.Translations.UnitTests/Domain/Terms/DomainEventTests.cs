using FluentAssertions;
using JetBrains.Annotations;
using Micro.Common.Domain;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Domain.TermAggregate.DomainEvents;

namespace Micro.Translations.UnitTests.Domain.Terms;

[TestSubject(typeof(Term))]
public class DomainEventTests
{
    private readonly Term _term;
    private readonly TermId _termId;
    private readonly TermName _termName;
    private readonly ProjectId _projectId;

    public DomainEventTests()
    {
        _termId = TermId.Create();
        _termName = TermName.Create("name");
        _projectId = ProjectId.Create();
        _term = Term.Create(_termId, _projectId, _termName);
    }
    
    [Fact]
    public void TermCreated_is_raised_on_creation()
    {
        _term.DomainEvents.Should().ContainSingle(x=> x is TermCreatedDomainEvent);
    }

    [Fact]
    public void TermNameUpdated_captures_both_old_and_new_names()
    {
        var newName = TermName.Create("new");
        _term.UpdateName(newName);
        _term.DomainEvents.Should().ContainSingle(x=> x is TermNameUpdatedDomainEvent);
        var @event = (TermNameUpdatedDomainEvent)_term.DomainEvents.Single(x => x is TermNameUpdatedDomainEvent);
        @event.OldName.Should().BeEquivalentTo(_termName);
        @event.NewName.Should().BeEquivalentTo(newName);
    }
    
    [Fact]
    public void TranslationUpdated_captures_both_old_and_new_text()
    {
        // arrange
        var au = Language.EnglishAustralian();
        var text1 = TranslationText.Create("text1");
        var text2 = TranslationText.Create("text2");
        
        // act
        _term.AddTranslation(au, text1);
        _term.UpdateTranslation(au, text2);
        
        // assert
        _term.DomainEvents.Should().ContainSingle(x=> x is TranslationUpdatedDomainEvent);
        var @event = (TranslationUpdatedDomainEvent)_term.DomainEvents.Single(x => x is TranslationUpdatedDomainEvent);
        @event.OldText.Should().BeEquivalentTo(text1);
        @event.NewText.Should().BeEquivalentTo(text2);
    }
}