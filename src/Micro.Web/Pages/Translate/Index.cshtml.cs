using Micro.Translations.Application.Queries;
using Micro.Translations.Infrastructure;
using Micro.Web.Code.Downloads;

namespace Micro.Web.Pages.Translate;

public class Index(ITranslationModule module, IPageContextAccessor context) : ContextualPageModel(context)
{
    public async Task OnGet()
    {
        Results = await module.SendQuery(new GetTranslationStatistics.Query());
    }
    
    public async Task<FileStreamResult> OnGetDownloadCsv(string languageCode, CancellationToken token)
    {
        var languageId = await module.SendQuery(new GetLanguage.Query(languageCode));
        var results = await module.SendQuery(new ListTranslations.Query(languageId));
        var stream = await FileDownloads.ToCsvMemoryStream(results, token);
        var name = $"{Org}-{Project}-{results.LanguageCode}.{FileDownloads.CsvExtension}";
        return new FileStreamResult(stream, FileDownloads.CsvContentType)
        {
            FileDownloadName = name
        };
    }
    
    public async Task<FileStreamResult> OnGetDownloadResx(string languageCode, CancellationToken token)
    {
        var languageId = await module.SendQuery(new GetLanguage.Query(languageCode));
        var results = await module.SendQuery(new ListTranslations.Query(languageId));
        var stream = FileDownloads.ToResXMemoryStream(results, token);
        var name = $"{Org}-{Project}-{results.LanguageCode}.{FileDownloads.ResxExtension}";
        return new FileStreamResult(stream, FileDownloads.TextContentType)
        {
            FileDownloadName = name
        };
    }

    public GetTranslationStatistics.Results Results { get; set; }
}