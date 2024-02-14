using System.Security.Claims;
using Micro.Tenants.Application.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Constants = Micro.Web.Code.Contexts.Constants;

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
            var result = await module.SendQuery(new CanAuthenticate.Query(Email, Password));
            if (!result.Success)
            {
                TempData.SetAlert(Alert.Danger("Login failed. Please try again."));
                return Page();
            }

            var claims = new Claim[]
            {
                new(Constants.UserClaimKey, result.UserId!.Value.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

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