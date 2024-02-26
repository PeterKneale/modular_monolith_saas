namespace Micro.Web.Pages.Auth;

public class Login(LoginService login, ILogger<Login> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await login.AuthenticateWithCredentials(Email, Password);
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