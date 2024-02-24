using Micro.Translations;
using static Micro.Translations.Application.Translations.ListTranslations;

namespace Micro.Web.Pages.Translate;

public class Translations(ITranslationModule module) : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public string LanguageCode { get; set; }

    public async Task OnGet()
    {
        var query = new Query(LanguageCode);
        Results = await module.SendQuery(query);
    }  
    
    public Result Results { get; set; }
}