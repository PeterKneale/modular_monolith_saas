using Micro.Common.Infrastructure.Behaviours;
using Micro.Common.Infrastructure.DomainEvents;

namespace Micro.Tenants.Infrastructure.Infrastructure.Database;

public class UnitOfWorkBehaviour<TRequest, TResponse>(Db db, DomainEventPublisher publisher, ILogger<Db> log) : BaseUnitOfWorkBehaviour<TRequest, TResponse>(db, publisher, log)
    where TRequest : notnull;