using Micro.Common.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Database;

public class BaseUnitOfWorkBehaviour<TRequest, TResponse>(DbContext db, DomainEventPublisher publisher, ILogger<DbContext> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // todo, add a check for IRequest<Unit> and return next() if not
        //if (request is not IRequest<Unit>) return await next();

        log.LogDebug("Beginning unit of work");
        var response = await next();
        await publisher.PublishDomainEvents(db, cancellationToken);

        log.LogDebug("Saving changes");
        await db.SaveChangesAsync(cancellationToken);

        log.LogDebug("Finished unit of work");
        return response;
    }
}