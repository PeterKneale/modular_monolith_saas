using Micro.Common.Infrastructure.Context;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations;
using Micro.Web.Code.Contexts;
using Micro.Web.Code.PageContext;

namespace Micro.Web.Pages.Shared.Components.OrgSelector;

public class OrgSelector(ITenantsModule module, IPageContextAccessor context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new Model
        {
            Memberships = await module.SendQuery(new ListOrganisations.Query())
        };

        if (context.Organisation == null)
        {
            return View(model);
        }
        
        // Get the current organisation
        model.Organisation = await module.SendQuery(new GetOrganisationByContext.Query());

        // Remove the current organisation from the list of memberships
        model.Memberships = model.Memberships.Where(x => x.OrganisationId != model.Organisation.Id);

        return View(model);
    }
}