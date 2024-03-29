namespace Micro.Common.Infrastructure.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logs) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).FullName.Split(".").Last();
        var body = JsonConvert.SerializeObject(request);

        if (name.StartsWith("Process"))
        {
            logs.LogDebug("Executing: {Name} - {Body}", name, body);
        }
        else
        {
            logs.LogInformation("Executing: {Name} - {Body}", name, body);
        }

        TResponse result;
        try
        {
            result = await next();
        }
        catch (Exception e)
        {
            logs.LogError(e,"Error Executing: {Name} - {Body}", name, body);
            throw;
        }

        return result;
    }
}