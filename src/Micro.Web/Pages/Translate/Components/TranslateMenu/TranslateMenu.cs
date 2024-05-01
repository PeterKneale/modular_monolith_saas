using Micro.Translations.Application.Queries;
using Micro.Translations.Infrastructure;

namespace Micro.Web.Pages.Translate.Components.TranslateMenu;

public class TranslateMenu(ITranslationModule module) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var languages = await module.SendQuery(new ListLanguages.Query());
        return View(new TranslateMenuModel
        {
            Languages = languages.Select(x=>x.Code)
        });
    }
}