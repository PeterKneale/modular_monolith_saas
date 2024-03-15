namespace Micro.AcceptanceTests.Pages.Layouts;

public class PageLayout(IPage page) 
{
    protected readonly IPage Page = page;
    
    public Components.PageId.Component PageId => new(Page);
    
    public Components.OrganisationSelector.Component OrganisationSelector => new(Page);

    public Components.ProjectSelector.Component ProjectSelector => new(Page);
    
    public Components.AlertComponent.Component Alert => new(Page);
}