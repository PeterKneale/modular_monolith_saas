﻿using Micro.Tenants.Application.Projects;
using static Micro.Web.Code.Contexts.Page.Constants;

namespace Micro.Web.Pages.Project;

public class Details(ITenantsModule module) : PageModel
{
    [BindProperty(SupportsGet = true, Name = ProjectRouteKey)]
    public string Name { get; set; }
    
    public async Task OnGetAsync()
    {
        Result = await module.SendQuery(new GetProjectByName.Query(Name));
    }

    public GetProjectByName.Result Result { get; set; }
}