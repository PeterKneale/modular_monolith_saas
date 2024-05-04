namespace Micro.Translations.Web.Pages.Terms;

public class IndexPage(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task OnGet()
    {
        Total = await module.SendQuery(new CountTerms.Query());
        Results = await module.SendQuery(new ListTerms.Query());
        Languages = await module.SendQuery(new ListLanguagesTranslated.Query());
    }


    public int Total { get; set; }

    public IEnumerable<ListTerms.Result> Results { get; set; }
    
    public IEnumerable<ListLanguagesTranslated.Result> Languages { get; set; }
}