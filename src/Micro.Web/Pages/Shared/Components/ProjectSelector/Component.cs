using Micro.Tenants.Application.Projects;
using Micro.Web.Code.Contexts;

namespace Micro.Web.Pages.Shared.Components.ProjectSelector;

public class ProjectSelector(ITenantsModule module) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!HttpContext.HasOrganisationId())
            return View(new Model{Show = false});

        var model = new Model
        {
            Show = true,
            Projects = await module.SendQuery(new ListProjects.Query())
        };

        if (HttpContext.HasProjectId())
        {
            var projectId = HttpContext.GetProjectId();

            model.CurrentProject = model.Projects.Single(x => x.ProjectId == projectId);

            // remove the current project from the list
            model.Projects = model.Projects.Where(x => x.ProjectId != projectId);
        }

        return View(model);
    }
}