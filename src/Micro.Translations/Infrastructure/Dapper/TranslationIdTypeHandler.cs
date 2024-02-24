using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Infrastructure.Dapper;

public class TranslationIdTypeHandler : SqlMapper.TypeHandler<TranslationId>
{
    private TranslationIdTypeHandler()
    {
    }

    public static readonly TranslationIdTypeHandler Default = new();

    public override TranslationId Parse(object? value)
    {
        if (value is Guid id)
        {
            return new TranslationId(id);
        }

        throw new FormatException($"Invalid conversion to {nameof(TranslationId)}");
    }

    public override void SetValue(IDbDataParameter parameter, TranslationId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}