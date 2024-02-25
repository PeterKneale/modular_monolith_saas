using Micro.Tenants.Application.ApiKeys.Queries;

namespace Micro.Web.Pages.ApiKeys;

public class Index(ITenantsModule module) : PageModel
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new List.Query());
    }

    public IEnumerable<List.Result> Results { get; set; }
}