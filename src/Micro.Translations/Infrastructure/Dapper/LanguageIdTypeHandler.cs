using Micro.Common.Infrastructure.Dapper;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Infrastructure.Dapper;

public class LanguageIdTypeHandler : SqlMapper.TypeHandler<LanguageId>
{
    private LanguageIdTypeHandler()
    {
    }

    public static readonly LanguageIdTypeHandler Default = new();

    public override LanguageId Parse(object? value)
    {
        if (value is Guid id)
        {
            return new LanguageId(id);
        }

        throw new FormatException($"Invalid conversion to {nameof(LanguageId)}");
    }

    public override void SetValue(IDbDataParameter parameter, LanguageId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}