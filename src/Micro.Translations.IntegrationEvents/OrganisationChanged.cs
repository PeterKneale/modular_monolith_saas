using Micro.Common.Infrastructure;
using Micro.Common.Infrastructure.Integration;

namespace Micro.Translations.IntegrationEvents;

public class TermChanged : IntegrationEvent
{
    public Guid TermId { get; init; }
    public string TermName { get; init; }
}