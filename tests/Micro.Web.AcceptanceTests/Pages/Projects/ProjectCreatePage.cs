using static Micro.Web.AcceptanceTests.Constants;

namespace Micro.Web.AcceptanceTests.Pages.Projects;

public class ProjectCreatePage(IPage page)
{
    private static string NameField => "Name";
    private static string CreateButton => "Create";

    public static ProjectCreatePage Goto(IPage page, string org)
    {
        page.GotoAsync(BaseUrl + $"{OrgRoute}/{org}/Projects/Create");
        return new ProjectCreatePage(page);
    }

    public async Task Create(string name)
    {
        await page.GetByTestId(NameField).FillAsync(name);
        await page.GetByTestId(CreateButton).ClickAsync();
    }
}