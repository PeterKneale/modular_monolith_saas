using System.Transactions;

namespace Micro.Common.Infrastructure.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var tx = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
            TransactionScopeAsyncFlowOption.Enabled);
        var response = await next();
        tx.Complete();
        return response;
    }
}