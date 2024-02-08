namespace Micro.Common.Infrastructure.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logs) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var name = typeof(TRequest).FullName;

            logs.LogError(ex, "{Name}: Unhandled Exception {@Request}", name, request);

            throw;
        }
    }
}