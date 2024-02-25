using Micro.Tenants.Application.Projects;
using Micro.Tenants.Application.Projects.Queries;

namespace Micro.Web.Pages.Shared.Components.ProjectSelector;

public class ProjectSelector(ITenantsModule module, IPageContextAccessor context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!context.HasOrganisation)
            return View(new Model { Show = false });

        var model = new Model
        {
            Show = true,
            OrganisationName = context.Organisation.Name,
            Projects = await module.SendQuery(new ListProjects.Query())
        };

        if (!context.HasProject)
        {
            return View(model);
        }
        
        var projectId = context.Project.Id;

        model.CurrentProject = model.Projects.Single(x => x.ProjectId == projectId);

        // remove the current project from the list
        model.Projects = model.Projects.Where(x => x.ProjectId != projectId);

        return View(model);
    }
}