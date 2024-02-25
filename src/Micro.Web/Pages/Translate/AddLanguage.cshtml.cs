using Micro.Translations;
using Micro.Translations.Application.Languages.Commands;

namespace Micro.Web.Pages.Translate;

public class AddLanguagePage(ITranslationModule module, IPageContextAccessor context, ILogger<AddLanguagePage> logs) : PageModel
{
    public IActionResult OnGet()
    {
        var languages = GetLanguages();
        Language = new SelectList(languages, "Value", "Text");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddLanguage.Command(Guid.NewGuid(), Selected));
            TempData.SetAlert(Alert.Success("You have added a new language"));
            return RedirectToPage(nameof(Index), new
            {
                Org = context.Organisation.Name,
                Project = context.Project.Name
            });
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    private static List<SelectListItem> GetLanguages() =>
    [
        new SelectListItem("Select One", ""),
        new SelectListItem("English (Australian)", "en-au"),
        new SelectListItem("English (United Kingdom)", "en-uk")
    ];

    [Display(Name = "Language")]
    public SelectList Language { get; set; }
    
    [Required]
    [BindProperty]
    public string Selected { get; set; }
}