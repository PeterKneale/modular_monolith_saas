using Micro.Common.Infrastructure.Database;
using Micro.Common.Infrastructure.DomainEvents;

namespace Micro.Tenants.Infrastructure.Database;

public class UnitOfWorkBehaviour<TRequest, TResponse>(Db db, DomainEventPublisher publisher, ILogger<Db> log) : BaseUnitOfWorkBehaviour<TRequest, TResponse>(db, publisher, log)
    where TRequest : IRequest<TResponse>;
