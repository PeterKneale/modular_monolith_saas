﻿using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Projects;

namespace Micro.Web.Code.Contexts;

public class ContextMiddleware(ITenantsModule tenants, ILogger<ContextMiddleware> logs) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // This is a middleware that will run for every request
        // It will check if the request has an organisation route value
        // If so, it will get the organisation from the database and add it to the HttpContext.Items

        logs.LogInformation("Setting web context");
        if (context.Request.RouteValues.TryGetValue(Constants.OrganisationRouteKey, out var organisationRoute))
        {
            var name = organisationRoute!.ToString()!;
            logs.LogInformation("Setting organisation context: {Name}", name);
            var result = await tenants.SendQuery(new GetOrganisationByName.Query(name));
            context.Request.HttpContext.SetOrganisationId(result.Id);
            context.Request.HttpContext.SetOrganisationName(result.Name);
        }
        else
        {
            logs.LogInformation("No organisation context detected");
        }

        if (context.Request.RouteValues.TryGetValue(Constants.ProjectRouteKey, out var projectRoute))
        {
            var name = projectRoute!.ToString()!;
            logs.LogInformation("Setting project context: {Name}", name);
            var result = await tenants.SendQuery(new GetProjectByName.Query(name));
            context.Request.HttpContext.SetProjectId(result.Id);
        }
        else
        {
            logs.LogInformation("No project context detected");
        }


        await next(context);
    }
}