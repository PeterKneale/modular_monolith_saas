using System.Transactions;

namespace Micro.Common.Infrastructure.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var trn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var response = await next();
        trn.Complete();
        return response;
    }
}