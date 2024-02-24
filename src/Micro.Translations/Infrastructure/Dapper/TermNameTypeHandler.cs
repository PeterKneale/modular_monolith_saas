using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Infrastructure.Dapper;

public class TermNameTypeHandler : SqlMapper.TypeHandler<TermName>
{
    private TermNameTypeHandler()
    {
    }

    public static readonly TermNameTypeHandler Default = new();

    public override TermName Parse(object? value)
    {
        if (value is string name)
        {
            return new TermName(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(TermName)}");
    }

    public override void SetValue(IDbDataParameter parameter, TermName? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Value;
    }
}