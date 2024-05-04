using Micro.Common.Web.Components;
using Micro.Users.Web.Contexts.Authentication;

namespace Micro.Users.Web.Pages.Auth;

public class Login(AuthenticationService authentication) : PageModel
{
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

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var result = await authentication.AuthenticateWithCredentials(Email, Password);
            if (!result)
            {
                TempData.SetAlert(Alert.Danger("Login failed. Please try again."));
                return Page();
            }

            TempData.SetAlert(Alert.Success("You have been successfully logged in."));
            return Redirect(returnUrl ?? "/");
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }
}