using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Organisations;
using Micro.Web.AcceptanceTests.Pages.Projects;
using Micro.Web.AcceptanceTests.UseCases.Auth;

namespace Micro.Web.AcceptanceTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SmokeTests : PageTest
{
    private const string Email = "TestUser@example.com";
    private const string Password = "password";

    [Test]
    public async Task Can_register_and_login_create_an_organisation()
    {
        var registerPage = RegisterPage.Goto(Page);
        var registerPageData = RegisterPageData.CreateValid() with
        {
            Email = Email,
            Password = Password
        };

        await registerPage.Register(registerPageData.FirstName, registerPageData.LastName, registerPageData.Email, registerPageData.Password);

        var loginPage = LoginPage.Goto(Page);
        await loginPage.Login(registerPageData.Email, registerPageData.Password);

        var orgCreatePage = OrganisationCreatePage.Goto(Page);
        var orgCreateData = OrganisationCreatePageData.CreateValid();
        await orgCreatePage.Create(orgCreateData.Name);

        var projectCreatePage = ProjectCreatePage.Goto(Page, orgCreateData.Name);
        var projectCreateData = ProjectCreatePageData.CreateValid();
        await projectCreatePage.Create(projectCreateData.Name);
    }
}