using Micro.Tenants.Application.Organisations;
using Index = Micro.Web.Pages.Translate.Index;

namespace Micro.Web.Pages.Organisations;

public class Create(ITenantsModule module) : PageModel
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
            TempData.SetAlert(Alert.Success("You have created a new organisation"));
            return RedirectToPage(nameof(Details), new {org = Name});
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