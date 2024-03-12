﻿using Micro.Translations;
using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Web.Pages.Translate.Translations;

public class UpdatePage(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    public async Task OnGet()
    {
        var query = new GetTranslation.Query(TermId, LanguageCode);
        var result = await module.SendQuery(query);
        Text = result.Text;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await module.SendCommand(new UpdateTranslation.Command(TermId, LanguageCode, Text));
            TempData.SetAlert(Alert.Success("You have updated a translation"));
            
            return RedirectToPage(nameof(Index), new
            {
                LanguageCode = LanguageCode,
                Org = context.Organisation.Name, 
                Project = context.Project.Name
            });
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    [Required] [BindProperty] public string Text { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string LanguageCode { get; set; }
}