using Micro.Common.Web.Components;
using DbConstants = Micro.Users.Infrastructure.DbConstants;

namespace Micro.Users.Web.Pages.Auth;

public class ForgotPassword(IUsersModule mediator) : PageModel
{
    [Display(Name = "Email")]
    [Required]
    [BindProperty]
    [StringLength(DbConstants.EmailMaxLength)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await mediator.SendCommand(new Application.Users.Commands.ForgotPassword.Command(Email));
            TempData.SetAlert(Alert.Success("Your password reset has been requested, please check your email"));
            return Redirect(nameof(Login));
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }
}