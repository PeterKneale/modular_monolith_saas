﻿using Micro.Common.Infrastructure.DomainEvents;

namespace Micro.Tenants.Infrastructure.Database;

public class UnitOfWorkBehaviour<TRequest, TResponse>(Db db, DomainEventPublisher mediator, ILogger<Db> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IRequest<Unit>)
        {
            log.LogInformation($"Begin query {request.GetType().FullName}");
            return await next();
        }

        log.LogInformation($"Begin command {request.GetType().FullName}");
        var response = await next();
        
        await mediator.PublishDomainEvents(db, cancellationToken);
        
        log.LogInformation("Saving changes");
        await db.SaveChangesAsync(cancellationToken);
        return response;
    }
}