using Micro.Translations;
using Micro.Translations.Application.Translations.Queries;

namespace Micro.Web.Pages.Translate;

public class Index(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new GetTranslationStatistics.Query());
    }

    public GetTranslationStatistics.Results Results { get; set; }
}