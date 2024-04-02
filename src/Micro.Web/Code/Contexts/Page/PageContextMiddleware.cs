using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Web.Code.Contexts.Page;

public class PageContextMiddleware(ITenantsModule tenants, ILogger<PageContextMiddleware> logs) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // This is a middleware that will run for every request
        // It will check if the request has an organisation route value
        // If so, it will get the organisation from the database and add it to the HttpContext.Items

        if (context.Request.RouteValues.TryGetValue(Constants.OrganisationRouteKey, out var organisationRoute))
        {
            var name = organisationRoute!.ToString()!;
            logs.LogDebug("Setting organisation context: {Name}", name);
            var result = await tenants.SendQuery(new GetOrganisationByName.Query(name));
            context.SetOrganisationContext(new PageContextOrganisation(result.Id, result.Name));
        }
        else
        {
            logs.LogDebug("No organisation context detected");
        }

        if (context.Request.RouteValues.TryGetValue(Constants.ProjectRouteKey, out var projectRoute))
        {
            var name = projectRoute!.ToString()!;
            logs.LogDebug("Setting project context: {Name}", name);
            var result = await tenants.SendQuery(new GetProjectByName.Query(name));
            context.SetProjectContext(new PageContextProject(result.Id, result.Name));
        }
        else
        {
            logs.LogDebug("No project context detected");
        }
        
        await next(context);
    }
}