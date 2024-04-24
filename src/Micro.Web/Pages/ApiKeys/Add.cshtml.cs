using Micro.Users;
using Micro.Users.Application.ApiKeys.Commands;
using Micro.Users.Application.ApiKeys.Queries;

namespace Micro.Web.Pages.ApiKeys;

public class AddPage(IUsersModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var id = Guid.NewGuid();
            await module.SendCommand(new CreateUserApiKey.Command(id, Name));
            var key = await module.SendQuery(new GetUserApiKeyById.Query(id));
            TempData.SetAlert(Alert.Success($"You have added a new API Key. Please copy it now as it will not be shown again. \n'{key}'"));
            return RedirectToPage(nameof(Index));
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    [Display(Name = "Name")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string Name { get; set; }
}