using Micro.Translations;
using static Micro.Translations.Application.Terms.ListTerms;

namespace Micro.Web.Pages.Translate;

public class Terms(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new Query());
    }  
    
    public Result Results { get; set; }
}