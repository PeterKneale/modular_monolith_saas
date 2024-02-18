using Micro.Translations;
using static Micro.Translations.Application.Translations.GetTranslationStatistics;

namespace Micro.Web.Pages.Translate;

public class Index(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        var command = new Query(Constants.ProjectId);
        Results = await module.SendQuery(command);
    }

    public Result Results { get; set; }
}