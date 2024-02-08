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
                new("UserId", "69ff55d7-b9cc-4786-a646-e1ee280d8b76"),
                new("OrganisationId", "96c89ebd-c639-4123-9e9f-b3a27350f936")
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