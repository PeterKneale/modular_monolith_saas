using Micro.Tenants.Application.Organisations;

namespace Micro.Web.Pages.Shared.Components.OrganisationSelector;

public class Model
{
    public GetOrganisationByContext.Result? Organisation { get; set; }
    public IEnumerable<ListOrganisations.Result> Memberships { get; set; }
}