namespace Micro.Common.Infrastructure.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logs) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).FullName;
        var body = JsonConvert.SerializeObject(request);

        logs.LogInformation("{Name} - {Body}", name, body);
        return await next();
    }
}