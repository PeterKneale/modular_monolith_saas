using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Terms.Commands;

namespace Micro.Web.Pages.Translate;

public class ImportTermsPage(ITranslationModule module, IPageContextAccessor context, ILogger<AddTermPage> logs) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await using var stream = ImportFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            var lines = (await reader.ReadToEndAsync()).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            await module.SendCommand(new ImportTerms.Command(lines));
            TempData.SetAlert(Alert.Success($"You have imported {lines.Length} term"));
            return RedirectToPage(nameof(Index), new
            {
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

    [BindProperty]
    public IFormFile ImportFile { get; set; }
}