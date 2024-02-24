namespace Micro.Translations.Domain.Terms;

public class Term(TermId id, ProjectId projectId, TermName name)
{
    public TermId Id { get; } = id;
    public ProjectId ProjectId { get; } = projectId;
    public TermName Name { get; } = name;
}