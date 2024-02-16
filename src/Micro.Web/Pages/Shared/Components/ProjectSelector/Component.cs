using Micro.Tenants.Application.Projects;
using Micro.Web.Code.Contexts;
using Micro.Web.Code.PageContext;

namespace Micro.Web.Pages.Shared.Components.ProjectSelector;

public class ProjectSelector(ITenantsModule module, IPageContextAccessor context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (context.Organisation == null)
            return View(new Model { Show = false });

        var model = new Model
        {
            Show = true,
            Projects = await module.SendQuery(new ListProjects.Query())
        };

        if (context.Project != null)
        {
            var projectId = context.Project.Id;

            model.CurrentProject = model.Projects.Single(x => x.ProjectId == projectId);

            // remove the current project from the list
            model.Projects = model.Projects.Where(x => x.ProjectId != projectId);
        }

        return View(model);
    }
}