using Micro.Common.Domain;
using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Translations;
using Micro.Web.Code;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Micro.Web.Pages.Translate;

public class AddTranslationPage(ITranslationModule module) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddTranslation.Command(Guid.NewGuid(), TermId, LanguageCode, Text));
            TempData.SetAlert(Alert.Success("You have added a new term"));
            return RedirectToPage(nameof(Translations), new { LanguageCode });
        }
        catch (BusinessRuleBrokenException e)
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
    public string LanguageCode { get; set; }
}