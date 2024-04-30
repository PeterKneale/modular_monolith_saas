using Micro.Translations.Application.Commands;
using Micro.Translations.Infrastructure;

namespace Micro.Web.Pages.Translate.Languages;

public class Add(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddLanguage.Command(Guid.NewGuid(), LanguageCode));
            TempData.SetAlert(Alert.Success("You have added a new language"));
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

    [Display(Name = "Language Code")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string LanguageCode { get; set; }
}