using Micro.Users;
using Micro.Users.Application.ApiKeys.Commands;
using Micro.Users.Application.ApiKeys.Queries;

namespace Micro.Web.Pages.User.ApiKeys;

public class Index(IUsersModule module) : PageModel
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new List.Query());
    }

    public async Task<IActionResult> OnPostDelete()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await module.SendCommand(new DeleteApiKey.Command(Id));

        TempData.SetAlert(Alert.Success("ApiKey has been deleted."));
        return RedirectToPage(nameof(Index));
    }

    public IEnumerable<List.Result> Results { get; set; }

    [BindProperty] [Required] public Guid Id { get; set; }
}