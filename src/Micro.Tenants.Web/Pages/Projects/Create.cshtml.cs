using Micro.Common.Web.Components;
using Micro.Common.Web.Contexts.PageContext;
using Micro.Tenants.Application.Organisations.Commands;

namespace Micro.Tenants.Web.Pages.Projects;

public class Create(ITenantsModule module, IPageContextOrganisation org) : PageModel
{
    [Display(Name = "Name")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string Name { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await module.SendCommand(new CreateProject.Command(Guid.NewGuid(), Name));
            TempData.SetAlert(Alert.Success("You have created a new project"));

            return RedirectToPage("/Project/Details", new { org = org.Name, project = Name });
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }
}