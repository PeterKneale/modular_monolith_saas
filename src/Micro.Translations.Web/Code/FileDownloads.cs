using System.Resources.NetStandard;
using System.Text;

namespace Micro.Translations.Web.Code;

public static class FileDownloads
{
    public static string CsvContentType => "text/csv";
    public static string TextContentType => "text/plain";

    public static string CsvExtension => "csv";
    public static string ResxExtension => "resx";

    public static async Task<MemoryStream> ToCsvMemoryStream(ListTranslations.Results results, CancellationToken token)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        foreach (var line in results.Translations)
        {
            await writer.WriteLineAsync($"{line.TermName},{line.TranslationText}");
        }

        await writer.FlushAsync(token);
        stream.Position = 0;
        return stream;
    }

    public static MemoryStream ToResXMemoryStream(ListTranslations.Results results, CancellationToken token)
    {
        var stream = new MemoryStream();
        var writer = new ResXResourceWriter(stream);
        writer.AddMetadata("language-code",results.Language.Code);
        foreach (var line in results.Translations)
        {
            writer.AddResource(line.TermName, line.TranslationText);
        }

        writer.Generate();
        stream.Position = 0;
        return stream;
    }
}