using Micro.Users;
using Micro.Users.Application.Users.Commands;
using Constants = Micro.Users.Constants;

namespace Micro.Web.Pages.Auth;

public class ForgotPasswordPage(IUsersModule mediator) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await mediator.SendCommand(new ForgotPassword.Command(Email));
            TempData.SetAlert(Alert.Success("Your password reset has been requested, please check your email"));
            return Redirect(nameof(LoginPage));
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
    [StringLength(Constants.MaxEmailLength)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}