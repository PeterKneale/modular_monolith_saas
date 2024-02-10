using Micro.Common.Domain;
using Micro.Tenants;
using Micro.Tenants.Application.Organisations;
using Micro.Web.Code;
using Index = Micro.Web.Pages.Translate.Index;

namespace Micro.Web.Pages.Organisations;

public class CreatePage(ITenantsModule module, ILogger<CreatePage> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new CreateOrganisation.Command(Guid.NewGuid(), Name));
            TempData.SetAlert(Alert.Success("You have added a new organisation"));
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
    public string Name { get; set; }
}