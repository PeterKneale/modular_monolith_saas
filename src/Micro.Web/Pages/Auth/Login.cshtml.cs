using System.Security.Claims;
using Micro.Tenants.Application.Users;
using Micro.Tenants.Domain.Users;
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
            var result = await module.SendQuery(new CanAuthenticate.Query(Email, Password));
            if (!result.Success)
            {
                TempData.SetAlert(Alert.Danger("Login failed. Please try again."));
                return Page();
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new("UserId", result.UserId!.Value.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme)));

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