namespace Micro.Web.AcceptanceTests.Pages.Projects;

public class ProjectDetailsPage(IPage page) : ProjectPageLayout(page)
{
    public static async Task<ProjectDetailsPage> Goto(IPage page, string organisation, string project)
    {
        await page.GotoAsync(BaseUrl + $"{OrgRoute}/{organisation}/projects/{project}");
        return new ProjectDetailsPage(page);
    }
}