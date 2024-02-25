namespace Micro.Web.Code.PageModels;

public class ContextualPageModel : PageModel
{
    private readonly IPageContextAccessor _context;

    protected ContextualPageModel(IPageContextAccessor context)
    {
        _context = context;
    }
    
    public string RouteOrg => _context.Organisation.Name;
    public string RouteProject => _context.Project.Name;
}