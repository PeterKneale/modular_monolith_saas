using Micro.Tenants.Application.Organisations.Queries;
using Micro.Tenants.Domain.OrganisationAggregate;
using Micro.Web.Code.Contexts.Authentication;

namespace Micro.Web.Pages.Shared.Components.Menu;

public class Menu(IAuthContext auth, IPageContextAccessor context, ITenantsModule module) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new Model
        {
            UserId = auth.UserId,
            Email = auth.Email,
            Organisations = new List<(Guid Id, string Name, bool IsCurrent)>(),
            Projects = new List<(Guid Id, string Name, bool IsCurrent)>()
        };

        GetOrganisationByContext.Result? currentOrganisation = null;
        if (context.HasOrganisation)
        {
            currentOrganisation = await module.SendQuery(new GetOrganisationByContext.Query());
        }

        var memberships = await module.SendQuery(new ListMemberships.Query());
        foreach (var membership in memberships)
        {
            var id = membership.OrganisationId;
            var name = membership.OrganisationName;
            var isCurrent = currentOrganisation != null && currentOrganisation.Id == id;
            model.Organisations.Add((id, name, isCurrent));
        }

        if (currentOrganisation != null)
        {
            GetProjectByContext.Result? currentProject = null;
            if (context.HasProject)
            {
                currentProject = await module.SendQuery(new GetProjectByContext.Query());
            }

            var projects = await module.SendQuery(new ListProjects.Query());
            foreach (var project in projects)
            {
                var id = project.Id;
                var name = project.Name;
                var isCurrent = currentProject != null && currentProject.Id == id;
                model.Projects.Add((id, name, isCurrent));
            }
        }

        return View(model);
    }
}