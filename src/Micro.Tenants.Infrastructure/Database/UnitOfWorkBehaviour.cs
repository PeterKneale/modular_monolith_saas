using Micro.Common.Infrastructure.Behaviours;
using Micro.Common.Infrastructure.DomainEvents;

namespace Micro.Tenants.Infrastructure.Database;

public class UnitOfWorkBehaviour<TRequest, TResponse>(Db db, DomainEventPublisher publisher)
    : BaseUnitOfWorkBehaviour<TRequest, TResponse>(db, publisher) where TRequest : notnull;