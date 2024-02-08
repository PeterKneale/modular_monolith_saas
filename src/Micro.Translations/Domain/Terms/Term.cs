using Micro.Common.Domain;

namespace Micro.Translations.Domain.Terms;

public class Term(TermId id, OrganisationId organisationId, AppId appId, TermName name)
{
    public TermId Id { get; } = id;
    public OrganisationId OrganisationId { get; } = organisationId;
    public AppId AppId { get; } = appId;
    public TermName Name { get; } = name;
}