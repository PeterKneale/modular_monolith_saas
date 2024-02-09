using Micro.Translations;
using static Micro.Translations.Application.Terms.ListTerms;

namespace Micro.Web.Pages.Translate;

public class Terms(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        var query = new Query(Constants.AppId);
        Results = await module.SendQuery(query);
    }  
    
    public Result Results { get; set; }
}