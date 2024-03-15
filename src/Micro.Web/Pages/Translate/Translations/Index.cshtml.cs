using Micro.Translations;
using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Web.Pages.Translate.Translations;

public class IndexPage(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public string LanguageCode { get; set; }

    public async Task OnGet()
    {
        var query = new ListTranslations.Query(LanguageCode);
        Results = await module.SendQuery(query);
    }

    public ListTranslations.Results Results { get; set; }

    public async Task<RedirectToPageResult> OnGetRemoveTranslation(Guid termId, string languageCode, CancellationToken token)
    {
        await module.SendCommand(new RemoveTranslation.Command(termId, languageCode));

        return RedirectToPage(nameof(Index), new
        {
            Org,
            Project,
            LanguageCode
        });
    }
}