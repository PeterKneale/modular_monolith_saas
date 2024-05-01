using Micro.Common.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Behaviours;

public class BaseUnitOfWorkBehaviour<TRequest, TResponse>(DbContext db, DomainEventPublisher publisher) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = GetName();
        
        if (name.Contains("query", StringComparison.InvariantCultureIgnoreCase))
        {
            return await next();
        }
        
        await using var tx = await db.Database.BeginTransactionAsync(cancellationToken);
        
        var response = await next();
        
        await publisher.PublishDomainEvents(db, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);
        
        await tx.CommitAsync(cancellationToken);

        return response;
    }

    private static string GetName() => typeof(TRequest).FullName!.Split(".").Last();
}