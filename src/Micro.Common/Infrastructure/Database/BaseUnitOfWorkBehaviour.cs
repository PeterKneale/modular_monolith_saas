using Micro.Common.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Database;

public class BaseUnitOfWorkBehaviour<TRequest, TResponse>(DbContext db, DomainEventPublisher publisher, ILogger<DbContext> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IRequest<Unit>)
        {
            return await next();
        }

        log.LogInformation("Beginning unit of work");
        var response = await next();
        await publisher.PublishDomainEvents(db, cancellationToken);
        
        log.LogInformation("Saving changes");
        await db.SaveChangesAsync(cancellationToken);
        
        log.LogInformation("Finished unit of work");
        return response;
    }
}