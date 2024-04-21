using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Micro.Web.Pages.Auth;

public class Logout : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        // Clear the existing external cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}