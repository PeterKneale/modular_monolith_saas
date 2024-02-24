using Micro.Translations.Domain;

namespace Micro.Translations.Infrastructure.Dapper;

public class LanguageCodeTypeHandler : SqlMapper.TypeHandler<LanguageCode>
{
    private LanguageCodeTypeHandler()
    {
    }

    public static readonly LanguageCodeTypeHandler Default = new();

    public override LanguageCode Parse(object? value)
    {
        if (value is string name)
        {
            return LanguageCode.FromIsoCode(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(LanguageCode)}");
    }

    public override void SetValue(IDbDataParameter parameter, LanguageCode? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Code;
    }
}