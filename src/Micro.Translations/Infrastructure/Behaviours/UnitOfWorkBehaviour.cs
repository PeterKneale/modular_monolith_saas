using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Behaviours;

public class UnitOfWorkBehaviour<TRequest, TResponse>(Db db, ILogger<Db> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        log.LogInformation($"Begin unit of work for {request.GetType().FullName} {db.ContextId}");
        var response = await next();
        log.LogInformation($"Saving unit of work {db.ContextId}");
        await db.SaveChangesAsync(cancellationToken);
        log.LogInformation($"Saved unit of work {db.ContextId}");
        return response;
    }
}