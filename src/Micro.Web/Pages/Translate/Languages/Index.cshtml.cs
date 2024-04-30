using Micro.Translations.Application.Queries;
using Micro.Translations.Infrastructure;

namespace Micro.Web.Pages.Translate.Languages;

public class Index(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new ListLanguages.Query());
    }
    
    public IEnumerable<string> Results { get; set; }
}