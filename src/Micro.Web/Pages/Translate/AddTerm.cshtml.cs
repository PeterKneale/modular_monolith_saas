using Micro.Common.Domain;
using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Web.Code;

namespace Micro.Web.Pages.Translate;

public class AddTermPage(ITranslationModule module, ILogger<AddTermPage> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new AddTerm.Command(Guid.NewGuid(),Constants.AppId,Term));
            TempData.SetAlert(Alert.Success("You have added a new term"));
            return RedirectToPage(nameof(Terms));
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    [Display(Name = "Name")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string Term { get; set; }
}