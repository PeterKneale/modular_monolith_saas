using Micro.Tenants.Application.Organisations;

namespace Micro.Web.Pages.Organisations;

public class Details(ITenantsModule module) : PageModel
{
    [BindProperty(SupportsGet = true, Name = "org")]
    public string Name { get; set; }
    
    public async Task OnGetAsync()
    {
        Result = await module.SendQuery(new GetOrganisationByName.Query(Name));
    }

    public GetOrganisationByName.Result Result { get; set; }
}