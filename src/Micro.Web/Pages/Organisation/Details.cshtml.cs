﻿using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Organisations.Queries;
using Constants = Micro.Web.Code.Contexts.Page.Constants;

namespace Micro.Web.Pages.Organisation;

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