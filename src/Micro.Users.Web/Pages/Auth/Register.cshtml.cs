using Micro.Common.Web.Components;
using Micro.Users.Application.Users.Commands;

namespace Micro.Users.Web.Pages.Auth;

public class Register(IUsersModule module, ILogger<Login> logs) : PageModel
{
    [Display(Name = "FirstName")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Display(Name = "LastName")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string LastName { get; set; }

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await module.SendCommand(new RegisterUser.Command(Guid.NewGuid(), FirstName, LastName, Email, Password));
            TempData.SetAlert(Alert.Success("You have been registered."));
            return Redirect("/");
        }
        catch (PlatformException e)
        {
            logs.LogWarning("Registration was not successful: {Message}", e.Message);
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }
}