namespace Micro.Web.Code.PageModels;

public class ContextualPageModel : PageModel
{
    private readonly IPageContextAccessor _context;

    protected ContextualPageModel(IPageContextAccessor context)
    {
        _context = context;
    }
    
    public string Org => _context.Organisation.Name;
    public string Project => _context.Project.Name;
}