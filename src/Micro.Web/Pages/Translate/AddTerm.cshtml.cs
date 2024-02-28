using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Terms.Commands;

namespace Micro.Web.Pages.Translate;

public class AddTermPage(ITranslationModule module, IPageContextAccessor context, ILogger<AddTermPage> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddTerm.Command(Guid.NewGuid(), Term));
            TempData.SetAlert(Alert.Success("You have added a new term"));
            return RedirectToPage(nameof(Index), new
            {
                Org = context.Organisation.Name,
                Project = context.Project.Name
            });
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
    public string Term { get; set; }
}