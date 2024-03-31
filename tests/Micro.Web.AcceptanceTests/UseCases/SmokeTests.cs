using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Organisations;
using Micro.Web.AcceptanceTests.Pages.Projects;

namespace Micro.Web.AcceptanceTests.UseCases;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SmokeTests : BaseTest
{
    [Test]
    public async Task Can_register_and_login_create_an_organisation_and_project()
    {
        var registerPage = await RegisterPage.Goto(Page);
        var user = RegisterPageData.CreateValid();

        await registerPage.Register(user.FirstName, user.LastName, user.Email, user.Password);
        await registerPage.Alert.AssertVisible();
        await registerPage.Alert.AssertLevelIsSuccess();
        
        await VerifyPage.Goto(Page, user.Email);
        
        var loginPage = await LoginPage.Goto(Page);
        await loginPage.Login(user.Email, user.Password);
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