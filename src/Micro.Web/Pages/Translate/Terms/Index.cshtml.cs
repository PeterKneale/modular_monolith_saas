using Micro.Translations;
using Micro.Translations.Application.Queries;
using static Micro.Translations.Application.Queries.ListTerms;

namespace Micro.Web.Pages.Translate.Terms;

public class IndexPage(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task OnGet()
    {
        Total = await module.SendQuery(new CountTerms.Query());
        Results = await module.SendQuery(new ListTerms.Query());
    }

    public int Total { get; set; }

    public IEnumerable<Result> Results { get; set; }
}