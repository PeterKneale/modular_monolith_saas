using Micro.AcceptanceTests.Pages.Auth;
using Micro.AcceptanceTests.Pages.Organisations;

namespace Micro.AcceptanceTests.UseCases.Auth;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class RegistrationTests: BaseTest
{
    [Test]
    public async Task Can_register_and_login_create_an_organisation()
    {
        var registerPage = await RegisterPage.Goto(Page);
        var registerPageData = RegisterPageData.CreateValid();

        await registerPage.Register(registerPageData.FirstName, registerPageData.LastName, registerPageData.Email, registerPageData.Password);
        
        var loginPage = await LoginPage.Goto(Page);
        await loginPage.Login(registerPageData.Email, registerPageData.Password);

        var createPage = await OrganisationCreatePage.Goto(Page);
        var createPageData = OrganisationCreatePageData.CreateValid();
        await createPage.Create(createPageData.Name);
    }
}