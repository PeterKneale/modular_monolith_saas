using System.Data;
using Dapper;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Dapper;

public class UserIdTypeHandler : SqlMapper.TypeHandler<UserId>
{
    private UserIdTypeHandler()
    {
    }

    public static readonly UserIdTypeHandler Default = new();

    public override UserId Parse(object? value)
    {
        if (value is Guid id)
        {
            return new UserId(id);
        }

        throw new FormatException($"Invalid conversion to {nameof(UserId)}");
    }

    public override void SetValue(IDbDataParameter parameter, UserId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}