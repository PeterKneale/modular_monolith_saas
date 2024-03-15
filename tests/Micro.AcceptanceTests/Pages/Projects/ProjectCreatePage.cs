using Micro.AcceptanceTests.Pages.Layouts;

namespace Micro.AcceptanceTests.Pages.Projects;

public class ProjectCreatePage(IPage page) : ProjectPageLayout(page)
{
    private static string NameField => "Name";
    private static string CreateButton => "Create";

    public static async Task<ProjectCreatePage> Goto(IPage page, string org)
    {
        await page.GotoRelativeUrlAsync($"{OrgRoute}/{org}/Projects/Create");
        return new ProjectCreatePage(page);
    }

    public async Task Create(string name)
    {
        await Page.GetByTestId(NameField).FillAsync(name);
        await Page.GetByTestId(CreateButton).ClickAsync();
    }
}