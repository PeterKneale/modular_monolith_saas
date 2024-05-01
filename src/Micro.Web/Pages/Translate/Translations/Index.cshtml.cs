using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;
using Micro.Translations.Infrastructure;

namespace Micro.Web.Pages.Translate.Translations;

public class IndexPage(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public string LanguageCode { get; set; }

    public async Task OnGet()
    {
        var language = await module.SendQuery(new GetLanguage.Query(LanguageCode));
        var query = new ListTranslations.Query(language.Id);
        Results = await module.SendQuery(query);
    }

    public ListTranslations.Results Results { get; set; }

    public async Task<RedirectToPageResult> OnGetRemoveTranslation(Guid termId, CancellationToken token)
    {
        var language = await module.SendQuery(new GetLanguage.Query(LanguageCode));
        await module.SendCommand(new RemoveTranslation.Command(termId, language.Id));

        return RedirectToPage(nameof(Index), new
        {
            Org,
            Project,
            LanguageCode
        });
    }
}