﻿using Micro.Common.Web.Components;
using Micro.Users.Web.Contexts.Authentication;

namespace Micro.Users.Web.Pages.Auth;

public class ResetPassword(IUsersModule mediator, ILogger<ResetPassword> logs) : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; } = null!;

    [Display(Name = "Password")]
    [Required]
    [BindProperty]
    [MinLength(AuthConstants.MinimumPasswordLength)]
    [MaxLength(AuthConstants.MaximumPasswordLength)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await mediator.SendCommand(new Application.Users.Commands.ResetPassword.Command(UserId, Token, Password));
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
}