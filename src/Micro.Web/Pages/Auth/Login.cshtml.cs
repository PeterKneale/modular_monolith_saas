using System.Security.Claims;
using Micro.Common.Domain;
using Micro.Tenants;
using Micro.Tenants.Domain.Users;
using Micro.Web.Code;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Micro.Web.Pages.Auth;

public class Login(ITenantsModule module, ILogger<Login> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new("UserId", "c0ee363f-5542-4f7e-930f-e7baa19afce2"),
                new("OrganisationId", "255e007f-0f46-4885-8b39-95b334e2acfc")
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            logs.LogInformation("Authentication was successful: {Email}", Email);
            
            TempData.SetAlert(Alert.Success("You have been successfully logged in."));
            return Redirect(returnUrl ?? "/");
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("Authentication was not successful: {Message}", e.Message);
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    [Display(Name = "Email")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}