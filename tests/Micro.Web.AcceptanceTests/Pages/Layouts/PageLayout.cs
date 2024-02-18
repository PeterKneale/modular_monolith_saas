namespace Micro.Web.AcceptanceTests.Pages.Layouts;

public class PageLayout(IPage page) 
{
    public Components.PageId.Component PageId => new(page);
    
    public Components.OrganisationSelector.Component OrganisationSelector => new(page);

    public Components.ProjectSelector.Component ProjectSelector => new(page);
    
    public Components.AlertComponent.Component Alert => new(page);
}