using Micro.Translations;
using Micro.Translations.Application.Commands;

namespace Micro.Web.Pages.Translate.Terms;

public class AddPage(ITranslationModule module, IPageContextAccessor context, ILogger<AddPage> logs) : PageModel
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