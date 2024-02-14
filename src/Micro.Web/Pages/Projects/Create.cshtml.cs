using Micro.Tenants.Application.Projects;
using Micro.Web.Code.Contexts;

namespace Micro.Web.Pages.Projects;

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
            await module.SendCommand(new CreateProject.Command(Guid.NewGuid(), Name));
            TempData.SetAlert(Alert.Success("You have created a new project"));
            
            return RedirectToPage(nameof(Details), new { org = this.Ctx().OrganisationName, project = Name });
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