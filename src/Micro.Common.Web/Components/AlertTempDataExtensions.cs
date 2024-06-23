using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Micro.Common.Web.Components;

public static class AlertTempDataExtensions
{
    private const string Key = "Alerts";

    public static bool HasAlert(this ITempDataDictionary dictionary) =>
        dictionary.ContainsKey(Key);

    public static void SetAlert(this ITempDataDictionary dictionary, Alert alert)
    {
        if (dictionary.HasAlert()) throw new Exception("Alert already set");
        var value = JsonConvert.SerializeObject(alert);
        dictionary.Add(Key, value);
    }

    public static Alert GetAlert(this ITempDataDictionary dictionary)
    {
        var json = dictionary[Key]!.ToString();
        return JsonConvert.DeserializeObject<Alert>(json!)!;
    }
}