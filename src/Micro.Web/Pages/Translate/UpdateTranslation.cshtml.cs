using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Translations;
using Micro.Translations.Application.Translations.Commands;
using Micro.Translations.Application.Translations.Queries;
using Micro.Translations.Domain.Translations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Micro.Web.Pages.Translate;

public class UpdateTranslationPage(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    public async Task OnGet()
    {
        var query = new GetTranslation.Query(TermId, LanguageId);
        var result = await module.SendQuery(query);
        Text = result.Text;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new UpdateTranslation.Command(TermId, LanguageId, Text));
            TempData.SetAlert(Alert.Success("You have updated a translation"));
            
            return RedirectToPage(nameof(Translations), new
            {
                LanguageId = LanguageId,
                Org = context.Organisation.Name, 
                Project = context.Project.Name
            });
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    [Required] [BindProperty] public string Text { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid LanguageId { get; set; }
}