using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Infrastructure.Infrastructure.Database.TypeHandlers;

public class LanguageIdTypeHandler : SqlMapper.TypeHandler<LanguageId>
{
    public static readonly LanguageIdTypeHandler Default = new();

    private LanguageIdTypeHandler()
    {
    }

    public override LanguageId Parse(object? value)
    {
        if (value is Guid id) return LanguageId.Create(id);

        throw new FormatException($"Invalid conversion to {nameof(LanguageId)}");
    }

    public override void SetValue(IDbDataParameter parameter, LanguageId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}