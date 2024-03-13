namespace Micro.Translations.Infrastructure.Database;

public class UnitOfWorkBehaviour<TRequest, TResponse>(Db db, ILogger<Db> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IRequest<Unit>)
        {
            return await next();
        }

        var response = await next();
        log.LogDebug("Saving changes");
        await db.SaveChangesAsync(cancellationToken);
        return response;
    }
}