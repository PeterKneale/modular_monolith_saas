using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Organisations;

namespace Micro.Web.AcceptanceTests.UseCases.Auth;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class RegistrationTests : PageTest
{
    [Test]
    public async Task Can_register_and_login_create_an_organisation()
    {
        var registerPage = RegisterPage.Goto(Page);
        var registerPageData = RegisterPageData.CreateValid();

        await registerPage.Register(registerPageData.FirstName, registerPageData.LastName, registerPageData.Email, registerPageData.Password);
        
        var loginPage = LoginPage.Goto(Page);
        await loginPage.Login(registerPageData.Email, registerPageData.Password);

        var createPage = CreatePage.Goto(Page);
        var createPageData = CreatePageData.CreateValid();
        await createPage.Create(createPageData.Name);
    }
}