﻿using System.ComponentModel;
using Micro.Common.Web.Components;
using Micro.Users.Application.Users.Commands;

namespace Micro.Users.Web.Pages.Auth;

public class VerifyEmail(IUsersModule module, ILogger<Login> logs) : PageModel
{
    [Required]
    [DisplayName("UserId")]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    [StringLength(50)]
    [DisplayName("Token")]
    public string Token { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!ModelState.IsValid) return Page();

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
}