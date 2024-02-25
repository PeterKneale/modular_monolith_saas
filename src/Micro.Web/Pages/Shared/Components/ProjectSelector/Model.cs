using Micro.Tenants.Application.Projects;
using Micro.Tenants.Application.Projects.Queries;

namespace Micro.Web.Pages.Shared.Components.ProjectSelector;

public class Model
{
    public string? OrganisationName { get; set; }
    public bool Show { get; set; }
    public ListProjects.Result? CurrentProject { get; set; }
    public IEnumerable<ListProjects.Result> Projects { get; set; }
}