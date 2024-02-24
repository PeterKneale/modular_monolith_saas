using Micro.Translations;
using Micro.Translations.Application.Terms.Queries;
using static Micro.Translations.Application.Terms.Queries.ListTerms;

namespace Micro.Web.Pages.Translate;

public class Terms(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        Total = await module.SendQuery(new CountTerms.Query());
        Results = await module.SendQuery(new ListTerms.Query());
    }

    public int Total { get; set; }

    public IEnumerable<Result> Results { get; set; }
}