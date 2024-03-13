using Micro.Common.Infrastructure.Integration;

namespace Micro.Translations.IntegrationEvents;

public class TermChanged(Guid termId, string termName) : IntegrationEvent
{
    public Guid TermId { get; init; } = termId;
    public string TermName { get; init; } = termName;
}