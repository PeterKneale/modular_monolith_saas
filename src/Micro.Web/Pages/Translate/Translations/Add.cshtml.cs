using Micro.Translations;
using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Web.Pages.Translate.Translations;

public class AddPage(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task OnGet()
    {
        var result = await module.SendQuery(new ListLanguages.Query());
        Languages = result.Select(x => new SelectListItem(x.Name, x.Code));
        if (LanguageCode != null)
        {
            foreach(var language in Languages)
            {
                if (language.Value == LanguageCode)
                {
                    language.Selected = true;
                }
            }
        }
    }
    
    [Required] 
    [BindProperty] 
    public string Text { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? LanguageCode { get; set; }

    public IEnumerable<SelectListItem> Languages { get; set; }

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
                Org,
                Project,
                LanguageCode
            });
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }
}