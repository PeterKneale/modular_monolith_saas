using Micro.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.DomainEvents;

public class DomainEventAccessor
{
    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents(DbContext db)
    {
        var entities = db.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Count != 0).ToList();

        return entities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents(DbContext db)
    {
        var domainEntities = db.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Count != 0).ToList();

        domainEntities
            .ForEach(entity => entity.Entity.ClearDomainEvents());
    }
}