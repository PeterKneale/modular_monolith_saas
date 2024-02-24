using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Constants = Micro.Web.Code.Contexts.Page.Constants;
using static Micro.Web.Code.Contexts.Page.Constants;

namespace Micro.Web.Code.TagHelpers;

[HtmlTargetElement("a")]
public class OrganisationLink(IPageContextAccessor accessor, ILogger<OrganisationLink> logs) : TagHelper()
{
    public override int Order =>-1;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.AllAttributes.ContainsName("asp-debug"))
        {
            logs.LogInformation("Start tag helper");
            foreach (var item in context.AllAttributes)
            {
                logs.LogInformation($"Existing attribute {item.Name} = {item.Value}");
            }

            foreach (var item in context.Items)
            {
                logs.LogInformation($"Existing context {item.Key} = {item.Value}");
            }

            if (accessor.HasOrganisation)
            {
                var key = $"asp-route-{OrganisationRouteKey}";
                var value = accessor.Organisation.Name;
                logs.LogInformation($"Adding {key} = {value} to link");
                var attributes = new TagHelperAttributeList(context.AllAttributes)
                {
                    { key, value }
                };
                context = new TagHelperContext(context.TagName, attributes, context.Items, context.UniqueId);
            }
            else
            {
                logs.LogInformation("No org context to add to link");   
            }
            
            if (accessor.HasProject)
            {
                var key = $"asp-route-{ProjectRouteKey}";
                var value = accessor.Project.Name;
                logs.LogInformation($"Adding {key} = {value} to link");
                var attributes = new TagHelperAttributeList(context.AllAttributes)
                {
                    { key, value }
                };
                context = new TagHelperContext(context.TagName, attributes, context.Items, context.UniqueId);
            }
            else
            {
                logs.LogInformation("No project context to add to link");   
            }
        }

        await base.ProcessAsync(context, output);
    }
}