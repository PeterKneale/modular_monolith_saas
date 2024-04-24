using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class OrganisationDetailsPage(IPage page) : OrganisationPageLayout(page)
{
    public static async Task<OrganisationCreatePage> Goto(IPage page, string name)
    {
        await page.GotoRelativeUrlAsync($"{OrgRoute}/{name}");
        return new OrganisationCreatePage(page);
    }
}