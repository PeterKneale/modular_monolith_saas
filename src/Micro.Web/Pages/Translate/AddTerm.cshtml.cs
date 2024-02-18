using Micro.Translations;
using Micro.Translations.Application.Terms;

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
            await module.SendCommand(new AddTerm.Command(Guid.NewGuid(), Constants.ProjectId, Term));
            TempData.SetAlert(Alert.Success("You have added a new term"));
            return RedirectToPage(nameof(Index));
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