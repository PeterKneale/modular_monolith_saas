using Micro.Users;
using Micro.Users.Application.ApiKeys.Queries;
using static Micro.Web.Code.Contexts.Authentication.Constants;

namespace Micro.Web.Code.Contexts.Authentication;

public class ApiKeyAuthenticationMiddleware(IUsersModule module, IHttpContextAccessor accessor, ILogger<ApiKeyAuthenticationMiddleware> log) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (accessor.HttpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }
        
        // check if the path starts with /api
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            log.LogDebug("Api path detected");
            
            // check if the request has the header
            if (context.Request.Headers.TryGetValue(ApiHeaderName, out var header))
            {
                log.LogDebug("Header detected: {Header}", header);
                var apiKey = header.ToString();
                log.LogDebug("ApiKey detected: {apiKey}", apiKey);
                var result = await module.SendQuery(new CanAuthenticate.Query(apiKey));
                if (result.Valid)
                {
                    var userId = result.UserId!.Value;
                    var email = result.Email!;
                    log.LogInformation("Authenticated using ApiKey. User: {UserId} ({UserEmail})", userId, email);
                    accessor.HttpContext.SetUserId(userId);
                    accessor.HttpContext.SetUserEmail(email);
                }
                else
                {
                    log.LogWarning("Invalid api key detected");
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized - Invalid ApiKey detected");
                    return;
                }
            }
            else
            {
                log.LogWarning("No header detected");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized - No ApiKey header detected");
                return;
            }
        }
        await next(context);
    }
}