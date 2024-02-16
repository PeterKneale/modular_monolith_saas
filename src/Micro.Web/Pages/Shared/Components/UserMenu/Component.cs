using Micro.Tenants.Application.Organisations;
using Micro.Web.Code.Contexts.Authentication;

namespace Micro.Web.Pages.Shared.Components.UserMenu;

public class UserMenu(IAuthContext context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new Model
        {
            UserId = context.UserId,
            Email = context.Email
        };
        return View(model);
    }
}