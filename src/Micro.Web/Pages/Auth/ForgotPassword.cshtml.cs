using DbConstants = Micro.Users.Infrastructure.DbConstants;

namespace Micro.Web.Pages.Auth;

public class ForgotPassword(IUsersModule mediator) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await mediator.SendCommand(new Users.Application.Users.Commands.ForgotPassword.Command(Email));
            TempData.SetAlert(Alert.Success("Your password reset has been requested, please check your email"));
            return Redirect(nameof(Login));
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
    [StringLength(DbConstants.MaxEmailLength)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}