using Micro.Common.Domain;

namespace Micro.Translations.Domain.Terms;

public class Term(TermId id, OrganisationId organisationId, ProjectId projectId, TermName name)
{
    public TermId Id { get; } = id;
    public OrganisationId OrganisationId { get; } = organisationId;
    public ProjectId ProjectId { get; } = projectId;
    public TermName Name { get; } = name;
}