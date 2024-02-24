using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Infrastructure.Dapper;

public class TranslationTextTypeHandler : SqlMapper.TypeHandler<TranslationText>
{
    private TranslationTextTypeHandler()
    {
    }

    public static readonly TranslationTextTypeHandler Default = new();

    public override TranslationText Parse(object? value)
    {
        if (value is string text)
        {
            return new TranslationText(text);
        }

        throw new FormatException($"Invalid conversion to {nameof(TranslationText)}");
    }

    public override void SetValue(IDbDataParameter parameter, TranslationText? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Value;
    }
}