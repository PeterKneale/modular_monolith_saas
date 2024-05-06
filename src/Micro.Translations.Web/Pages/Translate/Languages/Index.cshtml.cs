namespace Micro.Translations.Web.Pages.Translate.Languages;

public class Index(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new ListLanguages.Query());
    }
    
    public IEnumerable<ListLanguages.Result> Results { get; set; }
}