using Micro.Tenants.Application.Projects;

namespace Micro.Web.Pages.Projects;

public class Create(ITenantsModule module, IPageContextOrganisation org) : PageModel
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
            
            return RedirectToPage("/Project/Details", new { org = org.Name, project = Name });
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