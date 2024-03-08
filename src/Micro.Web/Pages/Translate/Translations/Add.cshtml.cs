using Micro.Translations;
using Micro.Translations.Application.Commands;

namespace Micro.Web.Pages.Translate.Translations;

public class AddPage(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    [Required] 
    [BindProperty] 
    public string Text { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string LanguageCode { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddTranslation.Command(TermId, LanguageCode, Text));
            TempData.SetAlert(Alert.Success("You have added a translation"));
            
            return RedirectToPage(nameof(Index), new
            {
                LanguageCode = LanguageCode,
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
}