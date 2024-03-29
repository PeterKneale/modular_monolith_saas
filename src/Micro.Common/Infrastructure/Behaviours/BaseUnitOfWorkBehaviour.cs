using Micro.Common.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Behaviours;

public class BaseUnitOfWorkBehaviour<TRequest, TResponse>(DbContext db, DomainEventPublisher publisher, ILogger<DbContext> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).FullName.Split(".").Last();
        
        if (name.Contains("query", StringComparison.InvariantCultureIgnoreCase))
        {
            log.LogDebug($"{name} Executing query");
            return await next();
        }

        log.LogDebug($"{name} - Beginning unit of work");
        await using var tx = await db.Database.BeginTransactionAsync(cancellationToken);
        log.LogDebug($"{name} - Started transaction {tx.TransactionId}");
        
        log.LogDebug($"{name} - Executing next step in pipeline");
        var response = await next();
        
        log.LogDebug($"{name} - Publishing domain events");
        await publisher.PublishDomainEvents(db, cancellationToken);

        log.LogDebug($"{name} - Saving changes");
        await db.SaveChangesAsync(cancellationToken);
        
        log.LogDebug($"{name} - Committing unit of work");
        await tx.CommitAsync(cancellationToken);

        return response;
    }
}