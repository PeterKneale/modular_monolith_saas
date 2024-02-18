namespace Micro.Web.AcceptanceTests.Pages.Projects;

public class ProjectCreatePage(IPage page) : ProjectPageLayout(page)
{
    private static string NameField => "Name";
    private static string CreateButton => "Create";

    public static async Task<ProjectCreatePage> Goto(IPage page, string org)
    {
        await page.GotoAsync(BaseUrl + $"{OrgRoute}/{org}/Projects/Create");
        return new ProjectCreatePage(page);
    }

    public async Task Create(string name)
    {
        await page.GetByTestId(NameField).FillAsync(name);
        await page.GetByTestId(CreateButton).ClickAsync();
    }
}