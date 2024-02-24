using Micro.Common.Infrastructure.Dapper;
using Micro.Translations.Domain;

namespace Micro.Translations.Infrastructure.Dapper;

public class TermIdTypeHandler : SqlMapper.TypeHandler<TermId>
{
    private TermIdTypeHandler()
    {
    }

    public static readonly TermIdTypeHandler Default = new();

    public override TermId Parse(object? value)
    {
        if (value is Guid id)
        {
            return new TermId(id);
        }

        throw new FormatException($"Invalid conversion to {nameof(TermId)}");
    }

    public override void SetValue(IDbDataParameter parameter, TermId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}