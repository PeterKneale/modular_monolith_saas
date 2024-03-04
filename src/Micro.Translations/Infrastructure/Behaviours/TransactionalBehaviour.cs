using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse>(Db db) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        await db.SaveChangesAsync(cancellationToken);
        return response;
    }
}