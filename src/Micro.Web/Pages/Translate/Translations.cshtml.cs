using Micro.Translations;
using Micro.Translations.Application.Translations.Commands;
using Micro.Translations.Application.Translations.Queries;

namespace Micro.Web.Pages.Translate;

public class Translations(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid LanguageId { get; set; }

    public async Task OnGet()
    {
        var query = new ListTranslations.Query(LanguageId);
        Results = await module.SendQuery(query);
    }  
    
    public ListTranslations.Results Results { get; set; }
    
    public async Task<RedirectToPageResult> OnGetRemoveTranslation(Guid termId, Guid languageId, CancellationToken token)
    {
        await module.SendQuery(new RemoveTranslation.Command(termId,languageId));
        
        return RedirectToPage(nameof(Translations), new
        {
            LanguageId = LanguageId,
            Org = context.Organisation.Name, 
            Project = context.Project.Name
        });
    }
}