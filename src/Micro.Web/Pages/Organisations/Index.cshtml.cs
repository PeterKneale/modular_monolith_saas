using Micro.Tenants.Application.Memberships;

namespace Micro.Web.Pages.Organisations;

public class Index(ITenantsModule module) : PageModel
{
    public async Task OnGetAsync()
    {
        Memberships = await module.SendQuery(new ListMemberships.Query());
    }

    public IEnumerable<ListMemberships.Result> Memberships { get; set; }
}