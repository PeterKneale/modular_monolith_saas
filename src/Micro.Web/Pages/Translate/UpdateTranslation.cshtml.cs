using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Translations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Micro.Web.Pages.Translate;

public class UpdateTranslationPage(ITranslationModule module) : PageModel
{
    public async Task OnGet()
    {
        var query = new GetTranslation.Query(Id);
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
            await module.SendCommand(new UpdateTranslation.Command(Id, Text));
            TempData.SetAlert(Alert.Success("You have updated a translation"));
            return RedirectToPage(nameof(Index));
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
    public Guid Id { get; set; }
}