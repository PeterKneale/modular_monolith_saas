using Micro.Users;
using Micro.Users.Application.Users.Commands;

namespace Micro.Web.Pages.Auth;

public class Reset(IUsersModule mediator, ILogger<Reset> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await mediator.SendCommand(new ResetPassword.Command(UserId, Token, Password));
            TempData.SetAlert(Alert.Success("Your password has been reset, please login to continue"));
            return Redirect(nameof(Login));
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("Resetting your password was not successful: {Message}", e.Message);
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    [Display(Name = "Password")]
    [Required]
    [BindProperty]
    [MinLength(AuthConstants.MinimumPasswordLength)]
    [MaxLength(AuthConstants.MaximumPasswordLength)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}