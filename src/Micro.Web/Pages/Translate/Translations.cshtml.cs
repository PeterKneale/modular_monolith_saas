using Micro.Translations;
using Micro.Translations.Application.Translations.Queries;

namespace Micro.Web.Pages.Translate;

public class Translations(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid LanguageId { get; set; }

    public async Task OnGet()
    {
        var query = new ListTranslations.Query(LanguageId);
        Results = await module.SendQuery(query);
    }  
    
    public ListTranslations.Results Results { get; set; }
}