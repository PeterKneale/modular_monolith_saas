using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.DomainEvents;

public class DomainEventPublisher(DomainEventAccessor accessor, IPublisher publisher, ILogger<DomainEventPublisher> log)
{
    public async Task PublishDomainEvents(DbContext db, CancellationToken cancellationToken)
    {
        var domainEvents = accessor.GetAllDomainEvents(db);
        foreach (var domainEvent in domainEvents)
        {
            log.LogInformation($"Publishing {domainEvent.GetType().Name}");
            await publisher.Publish(domainEvent, cancellationToken);
        }

        accessor.ClearAllDomainEvents(db);
    }
}