using Micro.Translations;
using static Micro.Translations.Application.Translations.GetTranslationSummary;

namespace Micro.Web.Pages.Translate;

public class Summary : PageModel
{
    private readonly ITranslationModule _module;

    public Summary(ITranslationModule module)
    {
        _module = module;
    }
    
    public async Task OnGet()
    {
        var appId = Guid.Parse("2f28d34a-26fc-4c9a-b16e-c2b5373c4f2b");
        var command = new Query(appId);
        Results = await _module.SendQuery(command);
    }

    public Result Results { get; set; }
}