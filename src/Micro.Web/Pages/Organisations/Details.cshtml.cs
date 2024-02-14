﻿using Micro.Tenants.Application.Organisations;
using Constants = Micro.Web.Code.Contexts.Constants;

namespace Micro.Web.Pages.Organisations;

public class Details(ITenantsModule module) : PageModel
{
    [BindProperty(SupportsGet = true, Name = Constants.OrganisationRouteKey)]
    public string Name { get; set; }
    
    public async Task OnGetAsync()
    {
        Result = await module.SendQuery(new GetOrganisationByName.Query(Name));
    }

    public GetOrganisationByName.Result Result { get; set; }
}