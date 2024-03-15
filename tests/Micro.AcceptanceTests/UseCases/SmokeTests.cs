using Micro.AcceptanceTests.Pages.Auth;
using Micro.AcceptanceTests.Pages.Organisations;
using Micro.AcceptanceTests.Pages.Projects;

namespace Micro.AcceptanceTests.UseCases;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SmokeTests : PageTest
{
    [Test]
    public async Task Can_register_and_login_create_an_organisation_and_project()
    {
        var registerPage = await RegisterPage.Goto(Page);
        var registerPageData = RegisterPageData.CreateValid();

        await registerPage.Register(registerPageData.FirstName, registerPageData.LastName, registerPageData.Email, registerPageData.Password);
        await registerPage.Alert.AssertVisible();
        await registerPage.Alert.AssertLevelIsSuccess();
        
        var loginPage = await LoginPage.Goto(Page);
        await loginPage.Login(registerPageData.Email, registerPageData.Password);
        await loginPage.Alert.AssertVisible();
        await loginPage.Alert.AssertLevelIsSuccess();

        var orgCreatePage = await OrganisationCreatePage.Goto(Page);
        var orgCreateData = OrganisationCreatePageData.CreateValid();
        await orgCreatePage.Create(orgCreateData.Name);
        await orgCreatePage.Alert.AssertVisible();
        await orgCreatePage.Alert.AssertLevelIsSuccess();

        var projectCreatePage = await ProjectCreatePage.Goto(Page, orgCreateData.Name);
        var projectCreateData = ProjectCreatePageData.CreateValid();
        await projectCreatePage.Create(projectCreateData.Name); 
        await projectCreatePage.Alert.AssertVisible();
        await projectCreatePage.Alert.AssertLevelIsSuccess();

    }
}