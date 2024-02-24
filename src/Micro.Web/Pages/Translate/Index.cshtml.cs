using Micro.Translations;
using Micro.Translations.Application.Translations;
using static Micro.Translations.Application.Translations.GetTranslationStatistics;

namespace Micro.Web.Pages.Translate;

public class Index(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new Query());
    }

    public GetTranslationStatistics.Results Results { get; set; }
}