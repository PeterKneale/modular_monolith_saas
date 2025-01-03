﻿using Micro.Common.Web.Components;
using Micro.Tenants.Application.Organisations.Commands;

namespace Micro.Tenants.Web.Pages.Organisations;

public class Create(ITenantsModule module) : PageModel
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
            await module.SendCommand(new CreateOrganisation.Command(Guid.NewGuid(), Name));
            TempData.SetAlert(Alert.Success("You have created a new organisation"));
            return RedirectToPage("/Organisation/Details", new { org = Name });
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}