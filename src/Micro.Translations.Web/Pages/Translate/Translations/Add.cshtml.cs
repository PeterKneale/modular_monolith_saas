namespace Micro.Translations.Web.Pages.Translate.Translations;

public class AddPage(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    [BindProperty(SupportsGet = true)] public string LanguageCode { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid TermId { get; set; }

    [Required] [BindProperty] public Guid LanguageId { get; set; }

    [Required] [BindProperty] public string Text { get; set; }

    public async Task OnGet()
    {
        var language = await module.SendQuery(new GetLanguage.Query(LanguageCode));
        LanguageId = language.Id;
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await module.SendCommand(new AddTranslation.Command(TermId, LanguageId, Text));
            TempData.SetAlert(Alert.Success("You have added a translation"));

            return RedirectToPage(nameof(Index), new
            {
                Org,
                Project,
                LanguageCode
            });
        }
        catch (PlatformException e)
        {
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }
}