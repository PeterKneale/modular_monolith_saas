using Micro.Tenants.Application.ApiKeys.Commands;
using Micro.Tenants.Application.ApiKeys.Queries;

namespace Micro.Web.Pages.ApiKeys;

public class Index(ITenantsModule module) : PageModel
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new ListUserApiKeys.Query());
    }

    public async Task<IActionResult> OnPostDelete()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await module.SendCommand(new DeleteUserApiKey.Command(Id));

        TempData.SetAlert(Alert.Success("ApiKey has been deleted."));
        return RedirectToPage(nameof(Index));
    }

    public IEnumerable<ListUserApiKeys.Result> Results { get; set; }

    [BindProperty] [Required] public Guid Id { get; set; }
}