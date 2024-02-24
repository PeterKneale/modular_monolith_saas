using Micro.Translations;
using Micro.Translations.Application.Translations.Commands;

namespace Micro.Web.Pages.Translate;

public class AddTranslationPage(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddTranslation.Command(Guid.NewGuid(), TermId, LanguageId, Text));
            TempData.SetAlert(Alert.Success("You have added a translation"));
            
            return RedirectToPage(nameof(Translations), new
            {
                LanguageId = LanguageId,
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

    [Required] [BindProperty] public string Text { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid LanguageId { get; set; }
}