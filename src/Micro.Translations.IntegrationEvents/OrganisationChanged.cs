using Micro.Common.Infrastructure.Outbox;

namespace Micro.Translations.IntegrationEvents;

public class TermChanged : IntegrationEvent
{
    public Guid TermId { get; init; }
    public string TermName { get; init; }
}