using Micro.Users;
using Micro.Users.Application.Users.Commands;

namespace Micro.Web.Pages.Auth;

public class VerifyPage(IUsersModule module, ILogger<LoginPage> logs) : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new VerifyUser.Command(UserId, Token));
            TempData.SetAlert(Alert.Success("You have been verified. Please login."));
            return Redirect("/Auth/Login");
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("Verification was not successful: {Message}", e.Message);
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }
    
    [Required]
    [BindProperty(SupportsGet = true)]
    [StringLength(50)]
    public string Token { get; set; }

}