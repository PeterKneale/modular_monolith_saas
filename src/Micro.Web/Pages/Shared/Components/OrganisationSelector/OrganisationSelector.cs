using Micro.Tenants.Application.Memberships.Queries;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Web.Pages.Shared.Components.OrganisationSelector;

public class OrganisationSelector(ITenantsModule module, IPageContextAccessor context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new Model
        {
            Memberships = await module.SendQuery(new ListMemberships.Query())
        };

        if (!context.HasOrganisation)
        {
            return View(model);
        }
        
        // Get the current organisation
        model.Organisation = await module.SendQuery(new GetOrganisation.Query());

        // Remove the current organisation from the list of memberships
        model.Memberships = model.Memberships.Where(x => x.OrganisationId != model.Organisation.Id);

        return View(model);
    }
}