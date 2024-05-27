namespace Micro.Translations.Web.Pages.Translate.Translations;

public class UpdatePage(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    [Required] [BindProperty] public string Text { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [BindProperty(SupportsGet = true)] public Guid LanguageId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string LanguageCode { get; set; }

    public async Task OnGet()
    {
        var language = await module.SendQuery(new GetLanguage.Query(LanguageCode));
        LanguageId = language.Id;
        var query = new GetTranslation.Query(TermId, LanguageId);
        var result = await module.SendQuery(query);
        Text = result.Text;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await module.SendCommand(new UpdateTranslation.Command(TermId, LanguageId, Text));
            TempData.SetAlert(Alert.Success("You have updated a translation"));

            return RedirectToPage(nameof(Index), new
            {
                LanguageCode,
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
}