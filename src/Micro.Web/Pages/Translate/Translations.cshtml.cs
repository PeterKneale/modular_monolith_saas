using Micro.Translations;
using static Micro.Translations.Application.Translations.ListTranslations;

namespace Micro.Web.Pages.Translate;

public class Translations(ITranslationModule module) : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public string Language { get; set; }

    public async Task OnGet()
    {
        var appId = Guid.Parse("2f28d34a-26fc-4c9a-b16e-c2b5373c4f2b");
        var query = new Query(appId, Language);
        Results = await module.SendQuery(query);
    }  
    
    public Result Results { get; set; }
}