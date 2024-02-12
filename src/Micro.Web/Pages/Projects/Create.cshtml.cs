using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Tenants.Application.Projects;

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
            
            // TODO: NEED ORG CONTEXT HERE
            // TODO: NEED ORG NAME IN CONTEXT (TECHNICALLY THE SLUG)
            
            
            return RedirectToPage(nameof(Details), new { org = "Alpha", project = Name });
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