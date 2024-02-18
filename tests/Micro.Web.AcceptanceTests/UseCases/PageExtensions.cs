using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Components.AlertComponent;
using Micro.Web.AcceptanceTests.Pages.Organisations;

namespace Micro.Web.AcceptanceTests.UseCases;

public static class PageExtensions
{
    public static async Task GivenLoggedIn(this IPage page)
    {
        var data = RegisterPageData.CreateValid();

        var register = await RegisterPage.Goto(page);

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);
        await register.AssertSuccessMessageShown();

        var login = await LoginPage.Goto(page);
        await login.Login(data.Email, data.Password);
        await login.AssertSuccessMessageShown();
    }

    public static async Task GivenOrganisationOwned(this IPage page, int count =1)
    {
        for (var i = 0; i < count; i++)
        {
            var data = OrganisationCreatePageData.CreateValid();
            var create = await OrganisationCreatePage.Goto(page);
            await create.Create(data.Name);
            await create.AssertSuccessMessageShown();
        }
    }
}