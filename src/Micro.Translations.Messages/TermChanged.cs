using Micro.Common.Infrastructure.Integration;

namespace Micro.Translations.Messages;

public class TermChanged(Guid termId, string termName) : IIntegrationEvent
{
    public Guid TermId { get; init; } = termId;
    public string TermName { get; init; } = termName;
}