using Micro.Tenants.Application.Memberships.Queries;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Web.Pages.Shared.Components.OrganisationSelector;

public class Model
{
    public GetOrganisation.Result? Organisation { get; set; }
    public IEnumerable<ListMemberships.Result> Memberships { get; set; }
}